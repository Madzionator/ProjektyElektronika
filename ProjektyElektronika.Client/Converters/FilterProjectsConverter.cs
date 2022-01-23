using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ProjektyElektronika.Client.Models;

namespace ProjektyElektronika.Client.Converters
{
    class FilterProjectsConverter : IMultiValueConverter
    {
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

            bool Filter(Project project)
            {
                if (project.Title.Contains(filterString, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (project.Author.Contains(filterString, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (project.AcademicYear.ToString().Contains(filterString, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (project.Category.Contains(filterString, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                return false;
            }

            return projects.Where(Filter).ToList();
        }

           
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
