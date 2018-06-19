//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eutyches.Spell.Hunspell;
using Eutyches.Spell.Lexedit.Services.Interfaces;
using Eutyches.Spell.Lexedit.Services.Models;
using Eutyches.Spell.Utilities;
using Prism.Events;
using Prism.Mvvm;

namespace Eutyches.Spell.Lexedit.Services
{
    public class FileService : BindableBase, IFileService
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private string _filePath = null;

        private bool _hasChanges = false;

        private Lexicon _lexicon = null;

        #endregion Fields

        #region Constructors

        public FileService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Properties

        public string FilePath
        {
            get => _filePath;
            protected set => SetProperty(ref _filePath, value, nameof(FilePath));
        }

        public bool HasChanges
        {
            get => _hasChanges;
            set => SetProperty(ref _hasChanges, value, nameof(HasChanges));
        }

        public bool HasFileName => !(string.IsNullOrWhiteSpace(FilePath));

        public bool IsLoaded => !(Lexicon is null);

        public Lexicon Lexicon
        {
            get => _lexicon;
            set => SetProperty(ref _lexicon, value, nameof(Lexicon));
        }

        #endregion Properties

        #region Methods

        public void ClearHasChanges()
        {
            HasChanges = false;
        }

        public void CloseLexicon()
        {
            var filePath = FilePath;

            Lexicon = null;
            FilePath = null;

            ClearHasChanges();

            _eventAggregator.GetEvent<FileClosedEvent>().Publish();
        }

        public void NewLexicon()
        {
            Lexicon = new Lexicon();
            FilePath = null;

            _eventAggregator.GetEvent<FileCreatedEvent>().Publish();
        }

        public Task<IEnumerable<Affix>> OpenAffixFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task OpenLexiconFileAsync(string filePath)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var zip = new ZippedLexiconFile(null);
                    Lexicon = zip.Read(filePath);
                }
                catch
                {
                    throw;
                }
            }).ContinueWith((e) =>
            {
                if(e.Exception is null)
                {
                    _eventAggregator.GetEvent<FileOpenedEvent>().Publish();
                    FilePath = filePath;
                }
                else
                {
                    _eventAggregator.GetEvent<FileErrorEvent>().Publish(e.Exception);

                    throw e.Exception;
                }
            });
        }

        public Task<IEnumerable<Stem>> OpenStemFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task SaveAffixFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task SaveLexiconFileAsync(string filePath)
        {
            if(string.IsNullOrWhiteSpace(filePath))
            {
                filePath = FilePath;
            }

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var zip = new ZippedLexiconFile(Lexicon);
                    zip.Write(filePath);
                }
                catch
                {
                    throw;
                }
            }).ContinueWith((e) =>
            {
                if(e.Exception is null)
                {
                    _eventAggregator.GetEvent<FileSavedEvent>().Publish();
                    FilePath = filePath;
                }
                else
                {
                    _eventAggregator.GetEvent<FileErrorEvent>().Publish(e.Exception);

                    throw e.Exception;
                }
            });
        }

        public Task SaveStemFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public void SetHasChanges()
        {
            HasChanges = true;
        }

        #endregion Methods
    }
}