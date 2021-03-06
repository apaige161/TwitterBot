﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace talkToTInvi
{
    class Program
    {
        static void Main(string[] args)
        {
            /*****TInvi helper program******/



            // Set up your credentials (https://apps.twitter.com)
            Auth.SetUserCredentials(Sensitive.apikey, Sensitive.apikeySecret, Sensitive.accesstoken, Sensitive.accesstokenSecret); 

            //get current time
            DateTime currentTime = DateTime.Now;

            //get the user added time && text to send from main program
            string userChoice = args[0]; //userchoice "3" text or "4" media
            Console.WriteLine(userChoice);
            if(userChoice == "3") // text
            {
                string textToTweet = args[1]; //grabs the text of tweet
                string newTimeDateString = args[2]; //grabs the date
                string newTimeTimeString = args[3]; //grabs the time
                string newTimeAmPmString = args[4]; //grabs AM or PM


                /* everything is formatted correctly for a media tweet but not for text*/
                Console.WriteLine(textToTweet + " is the textToTweet from main");//text
                Console.WriteLine(newTimeDateString + " is the date from main");//date
                Console.WriteLine(newTimeTimeString + " is the time from main");//time
                Console.WriteLine(newTimeAmPmString + " is the AM or PM from main"); //AM or PM

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("twitter bot is attempting to post a text tweet");
                Console.ResetColor();

                //add strings of DateTime data into a parseable form
                string newTimeString = newTimeDateString + " " + newTimeTimeString + " " + newTimeAmPmString;
                //parse to DateTime
                DateTime NewRealTime = DateTime.Parse(newTimeString);

                //display
                Console.WriteLine($"Your tweet will be published at {NewRealTime}");

                TextWorker(NewRealTime, currentTime, textToTweet);

            }

            if (userChoice == "4") // media
            {
                string textToTweet = args[1]; //grabs the text of tweet
                string filePath = args[2]; //grabs file path of picture
                string newTimeDateString = args[3]; //grabs the date
                string newTimeTimeString = args[4]; //grabs the time
                string newTimeAmPmString = args[5]; //grabs AM or PM

                /* everything is formatted correctly for a media tweet but not for text*/
                Console.WriteLine(textToTweet + " is the textToTweet from main");//text
                Console.WriteLine(filePath + " is the filepath from main");//date
                Console.WriteLine(newTimeDateString + " is the date from main");//time
                Console.WriteLine(newTimeTimeString + " is the time from main");//PM
                Console.WriteLine(newTimeAmPmString + " is the AM or PM from main"); //date

                //add strings of DateTime data into a parseable form
                string newTimeString = newTimeDateString + " " + newTimeTimeString + " " + newTimeAmPmString;
                //parse to DateTime
                DateTime NewRealTime = DateTime.Parse(newTimeString);

                //display
                Console.WriteLine($"Your tweet will be published at {NewRealTime}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("twitter bot is attempting to post a media tweet");
                Console.ResetColor();

                MediaWorker(NewRealTime, currentTime, textToTweet, filePath);
            }
            Console.ReadLine();
        }

        public static void TextWorker(DateTime newTime, DateTime currentTime, string textToTweet)
        {
            //datetime compare for while loop
            int timeCompare = DateTime.Compare(newTime, currentTime);

            /*  timeCompare:
             *  <0 − If date1 is earlier than date2
             *  0 − If date1 is the same as date2
             *  >0 − If date1 is later than date2
            */

            //waits until addTime is later than currentTime
            while (timeCompare > 0)
            {
                //end while loop, should end when timeCompare is changed
                Console.WriteLine("It is not time to post yet, the program will try again in 60 seconds...");
                Thread.Sleep(60000);   //wait for 60 seconds
                timeCompare = DateTime.Compare(newTime, DateTime.Now);

                /*****post tweet******/
                //runs program after while loop completes
                if (timeCompare < 0) //currentTime is later than addTime
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Tweet.PublishTweet(textToTweet);
                    Console.WriteLine("Tweet sent at " + DateTime.Now);
                    break;
                }
            }
        } //tweet text

        //tweet media

        public static void MediaWorker(DateTime newTime, DateTime currentTime, string textToTweet, string filePath)
        {
            //datetime compare for while loop
            int timeCompare = DateTime.Compare(newTime, currentTime);

            /*  timeCompare:
             *  <0 − If date1 is earlier than date2
             *  0 − If date1 is the same as date2
             *  >0 − If date1 is later than date2
            */

            //waits until addTime is later than currentTime
            while (timeCompare > 0)
            {
                //end while loop, should end when timeCompare is changed
                Console.WriteLine("It is not time to post yet, the program will try again in 30 seconds...");
                Thread.Sleep(30000);   //sleep for 30 seconds
                timeCompare = DateTime.Compare(newTime, DateTime.Now);
                Console.WriteLine($"{timeCompare} ");

                /*****post tweet******/
                //runs program after while loop completes
                if (timeCompare < 0) //currentTime is later than addTime
                {
                    //pass in the file to post from the main program
                    byte[] file1 = File.ReadAllBytes(filePath);
                    var media = Upload.UploadBinary(file1);
                    Tweet.PublishTweet(textToTweet + " " + DateTime.Now, new PublishTweetOptionalParameters
                    {
                        Medias = new List<IMedia> { media }
                    });
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tweet sent at " + DateTime.Now);
                    break;
                }

            }

        } //media tweet

    }
}
