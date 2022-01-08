using System;

namespace doancuoiky.Models 
{
    public class User {
        public int id {get; set;}
        public string name {get; set;}
        public string gender {get; set;}
        public DateTime birthday {get; set;}
        public int upvote {get; set;}
        public int greentick {get; set;}
        public string account {get; set;}
        public string password {get; set;}
        public int isCoach {get; set;}
        public string avatar {get; set;}
        public DateTime createDate {get; set;}
    }
}