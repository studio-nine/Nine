﻿namespace Nine.Studio.Shell.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Nine.Studio.Shell;           

    public class OldEditorView : INotifyPropertyChanged
    {
        #region Properties
        public IEditorShell Shell { get; private set; }
        public Editor Editor { get; private set; }
        public Project Project { get { return ActiveProject != null ? ActiveProject.Project : null; } }

        public bool IsViewVisible
        {
            get
            {
                return ActiveProjectItem != null &&
                      (ActiveProjectItem.Visualizers.Count > 1 ||
                      (ActiveProjectItem.ActiveVisualizer != null && ActiveProjectItem.ActiveVisualizer.Properties.Count > 0));
            }
        }

        public ProjectView ActiveProject
        {
            get { return activeProject; }
            set
            {
                if (value != activeProject)
                {
                    /*
                    if (value != null)
                    {
                        Verify.IsTrue(value.Editor == Editor, "The project is not owned by this editor");
                        Verify.IsTrue(Editor.Projects.Any(proj => proj == value.Project), "The project is not opened by this editor");
                    }

                    activeProject = value;
                    
                    Importers.Clear();
                    Exporters.Clear();
                    Factories.Clear();

                    if (activeProject != null)
                    {
                        Importers.AddRange(Editor.Extensions.Importers.Select(s => new ImporterView(this, s.Value, s.Metadata)));
                        Exporters.AddRange(Editor.Extensions.Exporters.Select(s => new ExporterView(this, s.Value, s.Metadata)));
                        Factories.AddRange(Editor.Extensions.Factories.Select(f => new FactoryView(this, f.Value, f.Metadata)));
                    }
                    */
                    NotifyPropertyChanged("FileName");
                    NotifyPropertyChanged("Title");
                    NotifyPropertyChanged("Name");
                    NotifyPropertyChanged("Project");
                    NotifyPropertyChanged("ActiveProject");

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }
        private ProjectView activeProject;

        /// <summary>
        /// Gets or sets the active document.
        /// </summary>
        public ProjectItemView ActiveProjectItem
        {
            get { return activeProjectItem; }
            set
            {
                if (value != activeProjectItem)
                {
                    if (value != null)
                    {
                        Verify.IsTrue(value.Project == Project, "The project item is not owned by this project");
                        Verify.IsTrue(Project.ProjectItems.Any(item => item.Project == value.Project), "The project item is not opened by this project");
                        
                    }

                    activeProjectItem = value;
                    SelectedObject = value != null ? value.ProjectItem.ObjectModel : null;
                    NotifyPropertyChanged("IsViewVisible");
                    NotifyPropertyChanged("ActiveProjectItem");
                }
            }
        }
        private ProjectItemView activeProjectItem;

        public object SelectedObject
        {
            get { return _SelectedObject; }
            set
            {
                if (value != _SelectedObject)
                {
                    _SelectedObject = value;
                    NotifyPropertyChanged("SelectedObject");
                }
            }
        }
        private object _SelectedObject;
        #endregion

        #region Initialization
        public OldEditorView(Editor editor, IEditorShell shell)
        {
            Verify.IsNotNull(editor, "editor");
            Verify.IsNotNull(shell, "shell");

            this.Shell = shell;
            this.Editor = editor;
            this.Editor.ProgressChanged += new ProgressChangedEventHandler(Editor_ProgressChanged);

            SaveCommand = new DelegateCommand(SaveProject, HasProject);
            CloseCommand = new DelegateCommand(CloseProject, HasProject);
            ExitCommand = new DelegateCommand(Exit);
            HelpCommand = new DelegateCommand(ShowHelp);

            UpdateSettings();
        }
        #endregion

        #region Commands
        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand HelpCommand { get; private set; }
        

        private bool HasProject()
        {
            return Project != null;
        }
            /*
            try
            {
                ProgressHelper.DoWork(
                    (e) =>
                    {
                        return Editor.OpenProject(fileName);
                    },
                    (e) =>
                    {
                        if (e != null)
                        {
                            ActiveProject = new ProjectView(this, (Project)e);
                        }
                    }, fileName, Strings.Loading, fileName);
            }
            catch (Exception ex)
            {
                Trace.TraceError(Strings.ErrorOpenProject);
                Trace.WriteLine(ex.ToString());
                MessageBox.Show(Strings.ErrorOpenProject, Editor.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
             */

        private void ImportProjectItem()
        {

        }
        
        private void ExportProjectItem()
        {
        }

        private void SaveProject()
        {
            ActiveProject.Save();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private async void CloseProject()
        {
            /*
            if (await EnsureProjectSavedAsync() && Project != null)
            {
                SelectedObject = null;
                Project.Close();
                ActiveProject = null;
                ActiveProjectItem = null;
            }
             */
        }

        private void Exit()
        {

        }

        private void ShowHelp()
        {
            OpenUrl("http://nine.codeplex.com");
        }
        #endregion

        #region Methods
        /// <summary>
        /// http://support.microsoft.com/kb/305703
        /// </summary>
        public void OpenUrl(string url)
        {
            try 
            {
                Process.Start(url); 
            } 
            catch
            {
                Trace.TraceError("Error opening url {0}", url);
            }
        }

        private void Editor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressHelper.UpdateState(Strings.Loading, e.UserState.ToString(), e.ProgressPercentage / 100.0);
        }

        private void UpdateSettings()
        {
            /*
            var settings = Editor.Extensions.FindSettings(typeof(Editor));
            if (ActiveDocument != null)
                settings = settings.Concat(Editor.Extensions.FindSettings(ActiveDocument.Document.DocumentObject.GetType()))
                                   .Distinct();

            var groupedSettings = settings.Select(s => new SettingsView(this, s))
                                          .GroupBy(v => v.Category)
                                          .Select(g => new SettingsView(this, g.First().DocumentSettings, g));

            Settings.AddRange(groupedSettings);
             */
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
