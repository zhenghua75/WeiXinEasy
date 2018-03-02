using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.JC
{
   public class JC_Quiz
    {
       private string iD;
       private string siteCode;
       private string quizType;
       private DateTime startTime;
       private string name;
       private string homeTeam;
       private string homeTeamImg;
       private string visitingTeam;
       private string visitingTeamImg;
       private string matchDesc;
       private string rightScore;
       private DateTime addTime;
       private int state;
       public string ID
       {
           get { return iD; }
           set { iD = value; }
       }
       public string SiteCode
       {
           get { return siteCode; }
           set { siteCode = value; }
       }
       public string QuizType
       {
           get { return quizType; }
           set { quizType = value; }
       }
       public DateTime StartTime
       {
           get { return startTime; }
           set { startTime = value; }
       }
       public string Name
       {
           get { return name; }
           set { name = value; }
       }
       public string HomeTeam
       {
           get { return homeTeam; }
           set { homeTeam = value; }
       }
       public string HomeTeamImg
       {
           get { return homeTeamImg; }
           set { homeTeamImg = value; }
       }
       public string VisitingTeam
       {
           get { return visitingTeam; }
           set { visitingTeam = value; }
       }
       public string VisitingTeamImg
       {
           get { return visitingTeamImg; }
           set { visitingTeamImg = value; }
       }
       public string MatchDesc
       {
           get { return matchDesc; }
           set { matchDesc = value; }
       }
       public string RightScore
       {
           get { return rightScore; }
           set { rightScore = value; }
       }
       public DateTime AddTime
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
}
