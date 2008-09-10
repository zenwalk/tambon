﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace De.AHoerstemeier.Tambon
{
    class CreationStatisticsTambon:CreationStatistics
    {
        #region properties
        private Int32 mNumberOfTambonCreations;
        public Int32 NumberOfTambonCreations { get { return mNumberOfTambonCreations; } }
        private FrequencyCounter mNumberOfMuban = new FrequencyCounter();
        private Int32[] mNumberOfTambonCreationsPerChangwat = new Int32[100];
        #endregion
        #region constructor
        public CreationStatisticsTambon()
        {
            StartYear = 1883;
            EndYear = DateTime.Now.Year;
        }
        public CreationStatisticsTambon(Int32 iStartYear, Int32 iEndYear)
        {
            StartYear = iStartYear;
            EndYear = iEndYear;
        }
        #endregion
        #region methods
        protected override void Clear()
        {
            base.Clear();
            mNumberOfMuban = new FrequencyCounter();
            mNumberOfTambonCreationsPerChangwat = new Int32[100];
            mNumberOfTambonCreations = 0;
        }
        protected override Boolean ContentFitting(RoyalGazetteContent iContent)
        { 
            Boolean retval = false;
            if (iContent is RoyalGazetteContentCreate)
            {
                RoyalGazetteContentCreate lCreate = (RoyalGazetteContentCreate)iContent;
                retval = (lCreate.Status == EntityType.Tambon);
            }
            return retval;
        }
        protected override void ProcessContent(RoyalGazetteContent iContent)
        {
            RoyalGazetteContentCreate lCreate = (RoyalGazetteContentCreate)iContent;
            mNumberOfTambonCreations++;

            Int32 lGeocode = lCreate.Geocode;
            while (lGeocode > 100)
            {
                lGeocode = lGeocode / 100;
            }
            mNumberOfTambonCreationsPerChangwat[lGeocode]++;

            Int32 lMaxMubanIndex = 0;
            foreach (RoyalGazetteContent lSubEntry in lCreate.SubEntities)
            {
                if (lSubEntry is RoyalGazetteContentReassign)
                {
                    Int32 lMubanCode = lSubEntry.Geocode % 100;
                    lMaxMubanIndex = Math.Max(lMaxMubanIndex, lMubanCode);
                }
            }

            mNumberOfMuban.IncrementForCount(lMaxMubanIndex,lCreate.Geocode);
        }
        public override String Information()
        {
            StringBuilder lBuilder = new StringBuilder();
            lBuilder.AppendLine(NumberOfAnnouncements.ToString() + " Announcements");
            lBuilder.AppendLine(NumberOfTambonCreations.ToString() + " Tambon created");

            if (mNumberOfTambonCreations > 0)
            {
                List<String> lChangwat = new List<String>();
                Int32 lMaxNumber = 1;
                foreach (PopulationDataEntry lEntry in Helper.Geocodes)
                {
                    if (mNumberOfTambonCreationsPerChangwat[lEntry.Geocode] > lMaxNumber)
                    {
                        lMaxNumber = mNumberOfTambonCreationsPerChangwat[lEntry.Geocode];
                        lChangwat.Clear();
                    }
                    if (mNumberOfTambonCreationsPerChangwat[lEntry.Geocode] == lMaxNumber)
                    {
                        lChangwat.Add(lEntry.English);
                    }
                }
                lBuilder.AppendLine(lMaxNumber.ToString() + " Tambon created in");
                foreach (String lName in lChangwat)
                {
                    lBuilder.AppendLine("* " + lName);
                }
                lBuilder.AppendLine();



                lBuilder.AppendLine("Most common number of muban: " + mNumberOfMuban.MostCommonValue.ToString() + " (" + mNumberOfMuban.MostCommonValueCount.ToString() + " times)");
                lBuilder.AppendLine("Mean number of muban: " + mNumberOfMuban.MeanValue.ToString());
                lBuilder.AppendLine("Standard deviation: " + mNumberOfMuban.StandardDeviation.ToString());
                lBuilder.AppendLine("Lowest number of muban: " + mNumberOfMuban.MinValue.ToString());
                if (mNumberOfMuban.MinValue > 0)
                {
                    foreach (Int32 lGeocode in mNumberOfMuban.Data[mNumberOfMuban.MinValue])
                    {
                        lBuilder.Append(lGeocode.ToString() + ' ');
                    }
                    lBuilder.AppendLine();
                }
                lBuilder.AppendLine("Highest number of muban: " + mNumberOfMuban.MaxValue.ToString());
                if (mNumberOfMuban.MaxValue > 0)
                {
                    foreach (Int32 lGeocode in mNumberOfMuban.Data[mNumberOfMuban.MaxValue])
                    {
                        lBuilder.Append(lGeocode.ToString() + ' ');
                    }
                    lBuilder.AppendLine();
                }
            }
            String retval = lBuilder.ToString();
            return retval;
        }

        #endregion

    }
}