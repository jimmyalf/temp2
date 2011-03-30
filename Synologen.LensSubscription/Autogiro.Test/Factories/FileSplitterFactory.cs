﻿using System;
using System.Collections.Generic;
using System.Text;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;

namespace Synologen.LensSubscription.Autogiro.Test.Factories
{
    public static class FileSplitterFactory
    {
        public static IEnumerable<string> GetFileNames()
        {
            return new List<string>
            {
                "UAGAG.K0999999.D110301.T130426",
                "UAGAG.K0999999.D000101.T000000",
                "UAGAG.K0999999.D991231.T235959"
            };
        }

        public static List<DateTime> GetDates()
        {
            return new List<DateTime>
                       {
                          new DateTime(2011, 3, 1, 13, 4, 26),
                          new DateTime(2000, 1, 1, 0, 0, 0),
                          new DateTime(1999, 12, 31, 23, 59, 59)
                       };
        }

        public static IEnumerable<string> GetValidAndInvalidFileNames()
        {
            return new List<string>
            {
                "UAGAG.K0999999.D110301.T130426",
                "UAGAG.K0999999.D000101.T000000",
                "UAGAG.K0999999.D991231.T235959",
                "IAGAG.K0999999.D991231.T235959",
                "testrapport.xls",
                "IAGAG.L0999999.D991231.T235959",
                "IAGAG.K1999999.D991231.T235959",
                "IAGAG.K0999991.D991231.T235959",
                "IAGAGK0999999D991231T235959",
                "",
                " IAGAGK0999999D991231T235959",
                "UAGAG.K0999999.110301.T130426",
                "UAGAG.K0999999.D110301.130426",
                "UAGAG.K0999999.D110301.T1304264",
                "UAGAG.K0999999.D1103013.T1304264"
            };
        }

        public static List<bool> GetBooleanResults()
        {
            return new List<bool>
                       {
                         true,
                         true,
                         true,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false,
                         false
                       };
        }

        public static string[] GetFileWithoutOpeningType()
        {
            return new []
            {
                "7300099123460000000000023344330012121212000019121212121200000041020041018000000", 
                "7300099123460000000000034433890132323211100000555600052100000043220041018041026",
                "7300099123460000000000042233500100123560000019680305111100000043220041018041026", 
                "7300099123460000000000052244700100000123456719460817222200000043220041018041026", 
                "7300099123460000000000044333600000123456777019490730444400000041020041018000000", 
                "7300099123460000195809010000                                 033320041018000000", 
                "092004101899000000007                                                          "
            };
            
        }

        public static string[] GetFileWithoutEndingType()
        {
            return new[]
            {
                "012004011899000009912346AG-MEDAVI                                              ",                                               
                "7300099123460000000000023344330012121212000019121212121200000041020041018000000", 
                "7300099123460000000000034433890132323211100000555600052100000043220041018041026",
                "7300099123460000000000042233500100123560000019680305111100000043220041018041026", 
                "7300099123460000000000052244700100000123456719460817222200000043220041018041026", 
                "7300099123460000000000044333600000123456777019490730444400000041020041018000000", 
                "7300099123460000195809010000                                 033320041018000000"
            };

        }

        public static string[] GetFileWithUnkownSectionType()
        {
            return new[]
            {
                "0120041015AUTOGIRO00000000000000000000000000000000000000                        ",
                "04000991234600000000000001013300001212121212191212121212                        ",
                "04000991234600000000000001028901003232323232005556000521                        ",
                "04000991234600000000000001035001000001000020196803050000                        ",
                "04000991234600000000000001029918000002010150194608170000                        ",
                "04000991234600000000000001029918000002010150194608170000                        ",
                "04000991234600000000000001029918000002010114193701270000                        ",
                "0300099123460000000000000242                                                    ",
                "09                                                                              "
            };
        }

        public static string[] GetFileWithMultipleSections()
        {
            return new[]
            {
                "0120041027AUTOGIRO9900                                        4711170009912346",  
                "82200410280    000000000000100100000002430000099123460809001                  ",
                "82200410280    000000000000100200000003840000099123460809002                  ",   
                "82200410280    000000000000100400000003350000099123460809004                  ",                                       
                "82200410280    000000000000100500000004620000099123460809005                  ",                                       
                "82200410280    000000000000100600000001720000099123460809006                  ",                                       
                "82200410280    000000000000100700000004840000099123460809007                  ",                                       
                "82200410280    000000000000100800000003140000099123460809008                  ",                    
                "82200410280    000000000000100900000001120000099123460809009                  ",                   
                "82200410280    000000000000101000000004870000099123460809010                  ",               
                "82200410280    000000000000101100000004340000099123460809011                  ",                 
                "82200410280    000000000000101200000003370000099123460809012                  ",                  
                "32200410280    000000000000101400000168740000099123460809745                  ",                   
                "82200410280    000000000000101300000002530000099123460809013                  2",
                "82200410280    000000000000101400000009690000099123460809014                  1",
                "82200410280    000000000000101500000004890000099123460809015                  9",
                "09200410279900              0000016874000000010000140000000000547500000000000000",
                "012004011899000009912346AG-MEDAVI                                              ",                                               
                "7300099123460000000000023344330012121212000019121212121200000041020041018000000", 
                "7300099123460000000000034433890132323211100000555600052100000043220041018041026",
                "7300099123460000000000042233500100123560000019680305111100000043220041018041026", 
                "7300099123460000000000052244700100000123456719460817222200000043220041018041026", 
                "7300099123460000000000044333600000123456777019490730444400000041020041018000000", 
                "7300099123460000195809010000                                 033320041018000000", 
                "092004101899000000007                                                          ",   
                "0120041022AUTOGIRO9900FELLISTA REG.KONTRL                     4711170009912346  ",
                "82200410230   0000000000000101000000050000                01                    ",                 
                "82200410230   0000000000000102000000020000                03                    ",                 
                "32200410230   0000000000000103000000010000                02                    ",                
                "82200410230   0000000000000104000000015000TESTREF         07                    ",               
                "09200410229900000001000000010000000003000000085000                 			 "                        
            };
        }

        public static IEnumerable<FileSection> GetExpectedSections()
        {
            return new List<FileSection>
             {
                 new FileSection
                     {
                         Posts = GetPaymentPosts(), 
                         SectionType = SectionType.ReceivedPayments
                     },
                 new FileSection
                     {
                         Posts = GetConsentPosts(), 
                         SectionType = SectionType.ReceivedConsents
                     },
                 new FileSection
                     {
                         Posts = GetErrorPosts(), 
                         SectionType = SectionType.ReceivedErrors
                     }
             };
        }

        private static string GetErrorPosts()
        {
            var sb = new StringBuilder();
            sb.AppendLine("0120041022AUTOGIRO9900FELLISTA REG.KONTRL                     4711170009912346  ");
            sb.AppendLine("82200410230   0000000000000101000000050000                01                    ");
            sb.AppendLine("82200410230   0000000000000102000000020000                03                    ");
            sb.AppendLine("32200410230   0000000000000103000000010000                02                    ");
            sb.AppendLine("82200410230   0000000000000104000000015000TESTREF         07                    ");
            sb.AppendLine("09200410229900000001000000010000000003000000085000                 			 ");
            return sb.ToString().TrimEnd('\r', '\n');
        }

        private static string GetConsentPosts()
        {
            var sb = new StringBuilder();
            sb.AppendLine("012004011899000009912346AG-MEDAVI                                              ");
            sb.AppendLine("7300099123460000000000023344330012121212000019121212121200000041020041018000000");
            sb.AppendLine("7300099123460000000000034433890132323211100000555600052100000043220041018041026");
            sb.AppendLine("7300099123460000000000042233500100123560000019680305111100000043220041018041026");
            sb.AppendLine("7300099123460000000000052244700100000123456719460817222200000043220041018041026");
            sb.AppendLine("7300099123460000000000044333600000123456777019490730444400000041020041018000000");
            sb.AppendLine("7300099123460000195809010000                                 033320041018000000");
            sb.AppendLine("092004101899000000007                                                          ");
            return sb.ToString().TrimEnd('\r', '\n');
        }

        private static string GetPaymentPosts()
        {
            var sb = new StringBuilder();
            sb.AppendLine("0120041027AUTOGIRO9900                                        4711170009912346");
            sb.AppendLine("82200410280    000000000000100100000002430000099123460809001                  ");
            sb.AppendLine("82200410280    000000000000100200000003840000099123460809002                  ");
            sb.AppendLine("82200410280    000000000000100400000003350000099123460809004                  ");
            sb.AppendLine("82200410280    000000000000100500000004620000099123460809005                  ");
            sb.AppendLine("82200410280    000000000000100600000001720000099123460809006                  ");
            sb.AppendLine("82200410280    000000000000100700000004840000099123460809007                  ");
            sb.AppendLine("82200410280    000000000000100800000003140000099123460809008                  ");
            sb.AppendLine("82200410280    000000000000100900000001120000099123460809009                  ");
            sb.AppendLine("82200410280    000000000000101000000004870000099123460809010                  ");
            sb.AppendLine("82200410280    000000000000101100000004340000099123460809011                  ");
            sb.AppendLine("82200410280    000000000000101200000003370000099123460809012                  ");
            sb.AppendLine("32200410280    000000000000101400000168740000099123460809745                  ");
            sb.AppendLine("82200410280    000000000000101300000002530000099123460809013                  2");
            sb.AppendLine("82200410280    000000000000101400000009690000099123460809014                  1");
            sb.AppendLine("82200410280    000000000000101500000004890000099123460809015                  9");
            sb.AppendLine("09200410279900              0000016874000000010000140000000000547500000000000000");
            return sb.ToString().TrimEnd('\r', '\n');
        }
    }
}
