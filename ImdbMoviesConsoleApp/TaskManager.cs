using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImdbMoviesConsoleApp
{
    public class TaskManager : IMovieObtainer
    {
        private List<Task<List<Movie>>> MovieTasks { get; set; }
        private MovieBatchGetter MovieGetter { get; set; }
        private int TaskCount { get; set; }

        public TaskManager()
        {
            MovieTasks = new List<Task<List<Movie>>>();
            MovieGetter = new MovieBatchGetter();
            TaskCount = Int32.Parse(ConfigReader.GetConfigValue("taskCount"));
        }

        public List<Movie> GetListOfMovies(int imdbIdForProcessing, int batchSize)
        {
            List<Movie> movies = new List<Movie>();
            List<Task<List<Movie>>> movieTasks = DefineTasks(TaskCount, imdbIdForProcessing, batchSize);
            WaitAllTasksInTasksList(movieTasks);
            GetResultsFromTasks(movieTasks, movies);
            return movies;
        }

        public List<Movie> GetListOfMovies(List<int> failedMoviesIds)
        {
            List<Movie> movies = new List<Movie>();
            List<Task<List<Movie>>> movieTasks = DefineTasks(TaskCount, failedMoviesIds);
            WaitAllTasksInTasksList(movieTasks);
            GetResultsFromTasks(movieTasks, movies);
            return movies;
        }

        private List<Task<List<Movie>>> DefineTasks(int countOfTask, int imdbIdForProcessing, int batchSize)
        {
            List<Task<List<Movie>>> movieTasks = new List<Task<List<Movie>>>();
            int imdbIdForTask = imdbIdForProcessing;
            for (int i = 0; i < countOfTask; i++)
            {
                imdbIdForTask = imdbIdForTask + (i == 0 ? 0 : batchSize);
                Logger.Instance.WriteLog($"TaskManager - imdbIdForTask: {imdbIdForTask} - batchSize: {batchSize}");
                movieTasks.Add(DefineTask(imdbIdForTask, batchSize));
            }
            return movieTasks;
        }

        private List<Task<List<Movie>>> DefineTasks(int countOfTask, List<int> failedMoviesIds)
        {
            List<Task<List<Movie>>> movieTasks = new List<Task<List<Movie>>>();
            int batchSize = failedMoviesIds.Count / countOfTask;
            for (int i = 0; i < countOfTask; i++)
            {
                movieTasks.Add(DefineTask(failedMoviesIds.Skip(i * batchSize).Take(batchSize).ToList()));
            }
            return movieTasks;
        }

        private Task<List<Movie>> DefineTask(int imdbIdForTask, int batchSize)
        {
            Task<List<Movie>> task = Task<List<Movie>>.Run(() => MovieGetter.GetMovies(imdbIdForTask, batchSize));
            return task;
        }

        private Task<List<Movie>> DefineTask(List<int> failedMoviesIds)
        {
            Task<List<Movie>> task = Task<List<Movie>>.Run(() => MovieGetter.GetMovies(failedMoviesIds));
            return task;
        }

        private void WaitAllTasksInTasksList(List<Task<List<Movie>>> movieTasks)
        {
            foreach (var task in movieTasks)
            {
                task.Wait();
            }
        }

        private void GetResultsFromTasks(List<Task<List<Movie>>> movieTasks, List<Movie> movies)
        {
            int counter = 0;
            foreach (var task in movieTasks)
            {
                counter++;
                List<Movie> taskMovies = task.Result;
                Logger.Instance.WriteLog($"Task{counter} result count: {taskMovies.Count}");
                movies.AddRange(taskMovies);
            }
        }
    }
}
