using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.JC
{
   public class JC_Score
    {
        private string iD;
        private string quizId;
        private string siteCode;
        private string openID;
        private string guessScore;
        private string addTime;
        private int state;
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string QuizId
        {
            get { return quizId; }
            set { quizId = value; }
        }
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
        public string OpenID
        {
            get { return openID; }
            set { openID = value; }
        }
        public string GuessScore
        {
            get { return guessScore; }
            set { guessScore = value; }
        }
        public string AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
        public int State
        {
            get { return state; }
            set { state = value; }
        }
    }

   public class MyQuiz
   {
       private string iD;
       private string quizId;
       private string homeTeam;
       private string visitingTeam;
       private string quizREsult;
       private string rightScore;
       private string quizType;
       private string guessScore;
       private int state;

       public string ID
       {
           get { return iD; }
           set { iD = value; }
       }
       public string QuizId
       {
           get { return quizId; }
           set { quizId = value; }
       }
       public string HomeTeam
       {
           get { return homeTeam; }
           set { homeTeam = value; }
       }
       public string VisitingTeam
       {
           get { return visitingTeam; }
           set { visitingTeam = value; }
       }
       public string QuizREsult
       {
           get { return quizREsult; }
           set { quizREsult = value; }
       }
       public string RightScore
       {
           get { return rightScore; }
           set { rightScore = value; }
       }
       public string QuizType
       {
           get { return quizType; }
           set { quizType = value; }
       }
       public string GuessScore
       {
           get { return guessScore; }
           set { guessScore = value; }
       }
       public int State
       {
           get { return state; }
           set { state = value; }
       }
   }
}
