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
        private const int FuzzySearchThreshold = 60;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var projects = (List<Project>)values[0];
                var filterString = (string)values[1];
                bool? isDiploma = (bool?)values[2] switch
                {
                    true => true,
                    false => null,
                    null => false
                };

                return UseFilter(projects, filterString, isDiploma);
            }
            catch
            {
                return null;
            }

        }

        private List<Project> UseFilter(List<Project> projects, string filterString, bool? isDiploma)
        {
            int Filter(Project project)
            {
                if (filterString is null or { Length: < 1 })
                {
                    return 100;
                }

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
                .Where(x=>isDiploma is null || isDiploma == x.IsDiploma)
                .Select(x=>(project:x, ratio:Filter(x)))
                .OrderByDescending(x=>x.ratio)
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
