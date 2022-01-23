using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FuzzySharp;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Converters
{
    class FilterProjectsConverter : IMultiValueConverter
    {
        private const int FuzzySearchThreshold = 50;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var projects = (List<Project>)values[0];
                var filterString = (string)values[1];

                return UseFilter(projects, filterString);
            }
            catch
            {
                return null;
            }

        }

        private List<Project> UseFilter(List<Project> projects, string filterString)
        {
            if (filterString is null or { Length: <1 }) 
            {
                return projects;
            }

            int Filter(Project project)
            {
                var searchString = $"{project.Title} | {project.AcademicYear} | {project.Author} | {project.Category}";
                int ratio = 0;
                
                if (searchString.Contains(filterString, StringComparison.CurrentCultureIgnoreCase))
                    return 100;

                var m1 = Fuzz.WeightedRatio(searchString, filterString);
                var m2 = Fuzz.WeightedRatio(project.Title, filterString);
                var m3 = Fuzz.WeightedRatio(project.Author, filterString);
                var m4 = Fuzz.WeightedRatio(project.Category, filterString);
                return Math.Max(m1, Math.Max(m2, Math.Max(m3, m4)));
            }

            return projects
                .Select(x=>(project:x, ratio:Filter(x)))
                .OrderByDescending(x=>x.ratio)
                 //.Select(x =>
                 //{
                 //    Debug.WriteLine($"{x.ratio} {x.project.Title} | {x.project.AcademicYear} | {x.project.Author} | {x.project.Category}");
                 //    return x;
                 //})
                //.Select(x =>
                //{
                //    x.project.Title = $"{x.ratio}% {x.project.Title}";
                //    return x;
                //})
                .Where(x=>x.ratio > FuzzySearchThreshold)
                .Select(x=>x.project)
                .ToList();
        }

           
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
