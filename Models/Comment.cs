using System;

namespace doancuoiky.Models 
{
    public class Comment {
        public int pid {get; set;}
        public int id {get; set;}
        public string description {get; set;}
        public int mcid {get; set;}
        public int owner {get; set;}
        public int greentick {get; set;}
    }
}