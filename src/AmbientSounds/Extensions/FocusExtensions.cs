﻿using AmbientSounds.Models;
using AmbientSounds.Services;
using JeniusApps.Common.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmbientSounds.Extensions
{
    public static class FocusExtensions
    {
        public static string ToCountdownFormat(this TimeSpan remaining)
        {
            return remaining.ToString(@"mm\:ss");
        }

        public static string ToDisplayString(this SessionType sessionType, ILocalizer localizer)
        {
            return localizer.GetString($"{nameof(SessionType)}{sessionType}");
        }

        public static TimeSpan GetTotalTime(int focusLength, int restLength, int repetitions)
        {
            if (focusLength < 0 ||
                restLength < 0 ||
                repetitions < 0)
            {
                return TimeSpan.Zero;
            }

            repetitions += 1;
            return TimeSpan.FromMinutes((focusLength + restLength) * repetitions);
        }

        //public static HistoryAward GetAward(this FocusHistory history, double? percentComplete = null)
        //{
        //    if (history is null)
        //    {
        //        return HistoryAward.None;
        //    }

        //    if (percentComplete is null)
        //    {
        //        percentComplete = GetPercentComplete(history);
        //    }

        //    if (percentComplete >= 100)
        //    {
        //        return HistoryAward.Gold;
        //    }
        //    else if (percentComplete >= 50)
        //    {
        //        return HistoryAward.Silver;
        //    }
        //    else if (history.FocusSegmentsCompleted >= 1)
        //    {
        //        return HistoryAward.Bronze;
        //    }

        //    return HistoryAward.None;
        //}

        public static double GetPercentComplete(this FocusHistory history)
        {
            // Percent complete is defined as number of focus minutes completed 
            // divided by the number of total possible focus minutes.

            if (history is null)
            {
                return 0;
            }

            int numOfRounds = history.Repetitions + 1;
            if (history.FocusSegmentsCompleted == numOfRounds)
            {
                return 100;
            }

            double totalFocusTime = history.FocusLength * numOfRounds;
            double focusTimeCompleted = history.FocusSegmentsCompleted * numOfRounds;
            if (history.PartialSegmentType == SessionType.Focus)
            {
                focusTimeCompleted += TimeSpan.FromTicks(history.PartialSegmentTicks).TotalMinutes;
            }

            return focusTimeCompleted / totalFocusTime * 100;
        }

        public static double GetFocusTimeTotal(this FocusHistory history)
        {
            return history.FocusLength * (history.Repetitions + 1);
        }

        public static double GetFocusTimeCompleted(this FocusHistory history)
        {
            if (history is null)
            {
                return 0;
            }

            double focusTimeCompleted = history.FocusLength * history.FocusSegmentsCompleted;
            if (history.PartialSegmentType == SessionType.Focus)
            {
                focusTimeCompleted += TimeSpan.FromTicks(history.PartialSegmentTicks).TotalMinutes;
            }

            return focusTimeCompleted;
        }

        public static double GetRestTimeCompleted(this FocusHistory history)
        {
            if (history is null)
            {
                return 0;
            }

            double restTimeCompleted = history.RestLength * history.RestSegmentsCompleted;
            if (history.PartialSegmentType == SessionType.Rest)
            {
                restTimeCompleted += TimeSpan.FromTicks(history.PartialSegmentTicks).TotalMinutes;
            }

            return restTimeCompleted;
        }
    }
}
