using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    class Program
    {
        static void Main(string[] args)
        {
            //greet user 
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.Rate = 1;
            synth.Speak("Welcome to Jarvis version 1.0");
            

            //change color
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            #region My Performance Counters

            //pull CPU usage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            //pull available RAM usage (in MB)
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            //pull uptime status (in seconds)
            PerformanceCounter perfUptime = new PerformanceCounter("System", "System Up Time");
            perfUptime.NextValue();

            //pull battery status
            //PerformanceCounter perfDiskStatus = new PerformanceCounter("% Disk Read Time", "PhysicalDisk");
            //perfDiskStatus.NextValue();

            #endregion

            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUptime.NextValue());

            //infinite while loop
            while (true) 
            {
                
                //get the current performance counter values
                int currentCPUPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();
                double currentUptimeStatus = (double)perfUptime.NextValue();
                //int currentDiskStatus = (int)perfDiskStatus.NextValue();
               
                //every 2 second print CPU usage to screen
                Console.WriteLine("CPU Load: {0}%", currentCPUPercentage );
                //every 2 second print available RAM in MB
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);
                //every 2 second print the up time
                Console.WriteLine ("Current Uptime: {0} Minutes", (currentUptimeStatus/60).ToString("n2"));
                //every 2 seconds print battery stats
                //Console.WriteLine("Battery Status: {0}", currentBatteryStatus);

               
                //warn user if CPU usage is greater than 80%
                if (currentCPUPercentage > 80)
                {
                    //warn of 100% CPU usage
                    if (currentCPUPercentage == 100)
                    {
                        
                        string cpuLoadVocalMessage = String.Format("Holy Crap your CPU is about to catch fire!");
                        synth.Speak(cpuLoadVocalMessage);
                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", currentCPUPercentage);
                        synth.Speak(cpuLoadVocalMessage);
                    }
                }
               
               //warn user if memory is below 1 GB
                if(currentAvailableMemory < 1024)
                {
                    string memAvailableMessage = String.Format("you currently have {0} megabytes of memory available", currentAvailableMemory);
                    synth.Speak(memAvailableMessage);

                }
                //end loop
                Thread.Sleep(2000);
            }
        }
    }
}
