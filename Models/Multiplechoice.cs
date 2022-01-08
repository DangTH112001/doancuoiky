using System;

namespace doancuoiky.Models 
{
    public class Multiplechoice {
        public int id {get; set;}
        public string title {get; set;}
        public string description {get; set;}
        public double rating {get; set;}
        public int time {get; set;}
        public int total {get; set;}
        public int status {get; set;}
        public int participant {get; set;}
        public DateTime createDate {get; set;}
        public string thumbnail {get; set;}
    }
}