using System;

[System.Serializable]
public class DailyGiftData {

    public int id;
    public string date;
    public int dc;
    public int Id { get => id; set => id = value; }
    public string Date { get => date; set => date = value; }
    public int DayCount { get => dc; set => dc = value; }

    public DailyGiftData() { 
        id = 0;
        date = DateTime.Now.ToString();
    }
    //public DailyGiftData(int i, string d)
    //{
    //    id = i;
    //    date = d;
    //}
}