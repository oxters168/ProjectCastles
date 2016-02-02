using UnityEngine;
using System.Collections;
using System;

public class Province {

    //public int provinceID { get; private set; }
    public string provinceName { get; private set; }
    public ProvinceScript provinceScript { get; private set; }
    public Noble owner { get; private set; }
    public Commodity commodity;
    //private string[] neighbours;

    //public Noble lastOwnerKnown { get; private set; }
    //public bool commodityKnown { get; private set; }

    public Province(string name, ProvinceScript pS, Commodity c, Noble o)
    {
        provinceName = name;
        provinceScript = pS;
        commodity = c;
        owner = o;
        //MakeNeighbours();
    }
    public Province(string name, ProvinceScript pS) : this(name, pS, RandomizeCommodity(), null) { }
    public Province(Province p) : this(p.provinceName, p.provinceScript, p.commodity, p.owner) { }

    public void SetOwnership(Noble n)
    {
        if (owner != null)
        {
            owner.Unclaim(this);
        }
        owner = n;
        //if (owner.playerControlled) Scout();
    }

    //public void Scout()
    //{
    //    commodityKnown = true;
    //    lastOwnerKnown = owner;
    //}

    public static Commodity RandomizeCommodity()
    {
        Array values = Enum.GetValues(typeof(Commodity));
        return (Commodity)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public string[] GetNeighbours()
    {
        if (provinceName.Equals("Albi", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Toulouse", "La Tour", "Quercy", "Narbonne" };
        if (provinceName.Equals("Amiens", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Rouen", "Maine", "Orleans", "Chartres", "Calais" };
        if (provinceName.Equals("Angouleme", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Rochefort", "Limoges", "Poitou" };
        if (provinceName.Equals("Auvergne", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Bergerac", "Tours", "La Marche", "Blois", "Nevers", "Bourbon", "Loire" };
        if (provinceName.Equals("Bergerac", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "La Tour", "Rochefort", "Limoges", "Tours", "Auvergne" };
        if (provinceName.Equals("Blois", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Auvergne", "La Marche", "Orleans", "Nevers" };
        if (provinceName.Equals("Bordeaux", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Rochefort", "Toulouse", "Gascony" };
        if (provinceName.Equals("Bourbon", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Loire", "Auvergne", "Nevers", "Orleans", "Dijon", "Valence", "Lyon" };
        if (provinceName.Equals("Brittany", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Vannes", "Normandy" };
        if (provinceName.Equals("Calais", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Amiens", "Chartres", "Reims", "Flanders" };
        if (provinceName.Equals("Chartres", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Orleans", "Amiens", "Calais", "Reims", "Dijon" };
        if (provinceName.Equals("Dijon", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Reims", "Chartres", "Orleans", "Bourbon", "Valence" };
        if (provinceName.Equals("Flanders", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Reims", "Calais" };
        if (provinceName.Equals("Gascony", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Bordeaux", "Toulouse" };
        if (provinceName.Equals("La Marche", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Auvergne", "Tours", "Maine", "Orleans", "Blois" };
        if (provinceName.Equals("La Tour", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Albi", "Toulouse", "Rochefort", "Bergerac", "Quercy" };
        if (provinceName.Equals("Limoges", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Rochefort", "Angouleme", "Poitou", "Tours", "Bergerac" };
        if (provinceName.Equals("Loire", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Quercy", "Auvergne", "Bourbon", "Lyon" };
        if (provinceName.Equals("Lyon", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Nimes", "Quercy", "Loire", "Bourbon", "Valence", "Provence" };
        if (provinceName.Equals("Maine", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Tours", "Nantes", "Rouen", "Amiens", "Orleans", "La Marche" };
        if (provinceName.Equals("Nantes", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Poitou", "Vannes", "Maine", "Tours" };
        if (provinceName.Equals("Narbonne", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Albi", "Quercy", "Nimes" };
        if (provinceName.Equals("Nevers", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Auvergne", "Blois", "Orleans", "Bourbon" };
        if (provinceName.Equals("Nimes", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Narbonne", "Quercy", "Lyon", "Provence" };
        if (provinceName.Equals("Normandy", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Vannes", "Brittany", "Rouen" };
        if (provinceName.Equals("Orleans", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Blois", "La Marche", "Maine", "Amiens", "Chartres", "Dijon", "Bourbon", "Nevers" };
        if (provinceName.Equals("Poitou", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Nantes", "Tours", "Limoges", "Angouleme" };
        if (provinceName.Equals("Provence", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Nimes", "Lyon", "Valence" };
        if (provinceName.Equals("Quercy", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Narbonne", "Albi", "La Tour", "Loire", "Lyon", "Nimes" };
        if (provinceName.Equals("Reims", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Flanders", "Calais", "Chartres", "Dijon" };
        if (provinceName.Equals("Rochefort", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Angouleme", "Limoges", "La Tour", "Toulouse", "Bordeaux" };
        if (provinceName.Equals("Rouen", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Normandy", "Vannes", "Maine", "Amiens" };
        if (provinceName.Equals("Toulouse", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Gascony", "Bordeaux", "Rochefort", "La Tour", "Albi" };
        if (provinceName.Equals("Tours", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Bergerac", "Limoges", "Poitou", "Nantes", "Maine", "La Marche", "Auvergne" };
        if (provinceName.Equals("Valence", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Provence", "Lyon", "Bourbon", "Dijon" };
        if (provinceName.Equals("Vannes", StringComparison.InvariantCultureIgnoreCase)) return new string[] { "Brittany", "Normandy", "Rouen", "Nantes" };
        return null;
    }
}
