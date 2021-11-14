using System;
using System.Collections.Generic;
using MvvmHelpers;

namespace ProjektyElektronika.Client.Models
{
    public class Project : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; }

        private DateTime? _dateCreated;
        public DateTime? DateCreated 
        { 
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }

        private ObservableRangeCollection<Author> _authors = new();
        public ObservableRangeCollection<Author> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }

        public string Address { get; set; }
        public bool IsDownloaded { get; set; }
    }
}
