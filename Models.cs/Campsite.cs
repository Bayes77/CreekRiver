

namespace CreekRiver.Models;

public class Campsite
{
    public int Id {get; set;}
    public string NickName {get; set;}
    public string ImageUrl {get; set;}
    public int CampsiteTypeId {get; set;}
    public CampsiteType campsiteType {get; set;}
    public List<Reservation> Reservations {get; set;}

}