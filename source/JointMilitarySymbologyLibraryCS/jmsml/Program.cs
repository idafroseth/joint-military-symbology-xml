﻿/* Copyright 2014 Esri
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JointMilitarySymbologyLibrary;

namespace jmsml
{
    static class Program
    {
        private static Librarian _librarian = new Librarian();

        static void Run(string[] args)
        {
            _librarian.IsLogging = true;

            //CommandLineArgs.I.parseArgs(args, "myStringArg=defaultVal;someLong=12");
            //Console.WriteLine("Arg myStringArg  : '{0}' ", CommandLineArgs.I.argAsString("myStringArg"));
            //Console.WriteLine("Arg someLong     : '{0}' ", CommandLineArgs.I.argAsLong("someLong"));

            CommandLineArgs.I.parseArgs(args, "");

            string exportPath = CommandLineArgs.I.argAsString("/x");
            string symbolSet = CommandLineArgs.I.argAsString("/s");
            string query = CommandLineArgs.I.argAsString("/q");
            string help = CommandLineArgs.I.argAsString("/?");
            string xPoints = CommandLineArgs.I.argAsString("/p");
            string xLines = CommandLineArgs.I.argAsString("/l");
            string xAreas = CommandLineArgs.I.argAsString("/a");

            if (help == "/?")
            {
                Console.WriteLine("jmsml.exe - Usage - Command line options are:");
                Console.WriteLine("");
                Console.WriteLine("/?\t\t\t: Help/Show command line options.");
                Console.WriteLine("/a\t\t\t: Export symbols with AREA geometry.");
                Console.WriteLine("/l\t\t\t: Export symbols with LINE geometry.");
                Console.WriteLine("/p\t\t\t: Export symbols with POINT geometry.");
                Console.WriteLine("/q=\"<expression>\"\t: Use regular expression to query on other labels.");
                Console.WriteLine("/s=\"<expression>\"\t: Use regular expression to query on symbol set labels.");
                Console.WriteLine("/x=\"<pathname>\"\t\t: Export to specified path (omit .csv).");
                Console.WriteLine("");
                Console.WriteLine("<Enter> to continue.");
                Console.ReadLine();
            }

            if (exportPath != "")
            {
                _librarian.Export(exportPath, symbolSet, query, xPoints == "/p" || xLines == "" && xAreas == "", 
                                                                xLines == "/l" || xPoints == "" && xAreas == "",
                                                                xAreas == "/a" || xPoints == "" && xLines == "");
            }
        }

        static int Main(string[] args)
        {
            // TODO Use a more robust arguments parser
            if (args.Any(arg => arg.Equals("/v") || arg.Equals("-v"))) // verbose?
                Trace.Listeners.Add(new ConsoleTraceListener(true));

            try
            {
                Run(args);
                return Environment.ExitCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Trace.TraceError(e.ToString());

                return Environment.ExitCode != 0
                     ? Environment.ExitCode : 100;
            }
        }
    }
}
