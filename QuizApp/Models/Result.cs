using System;
namespace QuizApp.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int AttemptedQuestions { get; set; }
        public int WrongAnswers { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}