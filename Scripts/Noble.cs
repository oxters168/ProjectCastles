using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Noble {

    //public bool playerControlled { get; private set; }
    public NobleScript nobleScript { get; private set; }
    public Color nobleColor { get; private set; }

    public string name { get; private set; }

    public int administrativePoints { get; private set; }
    public int administrativePointsLeft { get; private set; }
    public int militaryPoints { get; private set; }
    public int militaryPointsLeft { get; private set; }
    public int politicalPoints { get; private set; }
    public int politicalPointsLeft { get; private set; }

    public int infantry { get; private set; }
    public int archers { get; private set; }
    public int knights { get; private set; }

    public int food { get; private set; }
    public int timber { get; private set; }
    public int iron { get; private set; }
    public int gold { get; private set; }

    public float scoutTime = 5f;
    public float attackTime = 10f;

    private List<Province> ownedLands = new List<Province>();
    private Dictionary<string, Province> scoutedLands = new Dictionary<string, Province>();
    private List<string> scouting = new List<string>(), attacking = new List<string>();

    public Noble(NobleScript nS, Color nC, string n, int aP, int mP, int pP, int inf, int a, int k, int f, int t, int i, int g)
    {
        //playerControlled = pC;
        nobleScript = nS;
        nobleColor = nC;

        name = n;

        administrativePoints = aP;
        administrativePointsLeft = administrativePoints;
        militaryPoints = mP;
        militaryPointsLeft = militaryPoints;
        politicalPoints = pP;
        politicalPointsLeft = politicalPoints;

        infantry = inf;
        archers = a;
        knights = k;

        food = f;
        timber = t;
        iron = i;
        gold = g;

        //ownedLands = new List<Province>();
        //scoutedLands = new Dictionary<string, Province>();
    }
    public Noble(NobleScript nS, string n) : this(nS, ProvinceScript.noOwnerShieldColor, n, 4, 4, 4, 4, 3, 0, 10, 10, 10, 10) { }
    //public Noble(string n) : this(ProvinceScript.noOwnerShieldColor, n, 4, 3, 0, 10, 10, 10, 10) { }
    public Noble(NobleScript nS, Color nC, string n) : this(nS, nC, n, 4, 4, 4, 4, 3, 0, 10, 10, 10, 10) { }
    //public Noble(Color nC, string n) : this(nC, n, 4, 3, 0, 10, 10, 10, 10) { }

    public IEnumerator StartScout(Province p, int aP, int mP, int pP)
    {
        int adminPointsUsed = aP, militaryPointsUsed = mP, politicalPointsUsed = pP;
        if (adminPointsUsed > administrativePointsLeft) adminPointsUsed = administrativePointsLeft;
        else if (adminPointsUsed < 0) adminPointsUsed = 0;
        if (militaryPointsUsed > militaryPointsLeft) militaryPointsUsed = militaryPointsLeft;
        else if (militaryPointsUsed < 0) militaryPointsUsed = 0;
        if (politicalPointsUsed > politicalPointsLeft) politicalPointsUsed = politicalPointsLeft;
        else if (politicalPointsUsed < 0) politicalPointsUsed = 0;

        if (scouting.IndexOf(p.provinceName) < 0 && politicalPointsUsed >= 1)
        {
            administrativePointsLeft -= adminPointsUsed;
            militaryPointsLeft -= militaryPointsUsed;
            politicalPointsLeft -= politicalPointsUsed;

            scouting.Add(p.provinceName);
            yield return new WaitForSeconds(scoutTime / (adminPointsUsed + militaryPointsUsed + politicalPointsUsed));
            ScoutLand(p);
            scouting.Remove(p.provinceName);

            administrativePointsLeft += adminPointsUsed;
            militaryPointsLeft += militaryPointsUsed;
            politicalPointsLeft += politicalPointsUsed;
        }
    }
    public IEnumerator StartAttack(Province p, int aP, int mP, int pP)
    {
        int adminPointsUsed = aP, militaryPointsUsed = mP, politicalPointsUsed = pP;
        if (adminPointsUsed > administrativePointsLeft) adminPointsUsed = administrativePointsLeft;
        else if (adminPointsUsed < 0) adminPointsUsed = 0;
        if (militaryPointsUsed > militaryPointsLeft) militaryPointsUsed = militaryPointsLeft;
        else if (militaryPointsUsed < 0) militaryPointsUsed = 0;
        if (politicalPointsUsed > politicalPointsLeft) politicalPointsUsed = politicalPointsLeft;
        else if (politicalPointsUsed < 0) politicalPointsUsed = 0;

        if (attacking.IndexOf(p.provinceName) < 0 && militaryPointsUsed >= 2)
        {
            administrativePointsLeft -= adminPointsUsed;
            militaryPointsLeft -= militaryPointsUsed;
            politicalPointsLeft -= politicalPointsUsed;

            attacking.Add(p.provinceName);
            yield return new WaitForSeconds(attackTime / (adminPointsUsed + militaryPointsUsed + politicalPointsUsed));
            ClaimLand(p);
            attacking.Remove(p.provinceName);

            administrativePointsLeft += adminPointsUsed;
            militaryPointsLeft += militaryPointsUsed;
            politicalPointsLeft += politicalPointsUsed;
        }
    }
    public void ClaimLand(Province p)
    {
        if (ownedLands.IndexOf(p) < 0)
        {
            p.SetOwnership(this);
            RemoveScoutedLand(p);
            ownedLands.Add(p);
        }
    }
    public void Unclaim(Province p)
    {
        if (ownedLands.IndexOf(p) > -1)
        {
            ownedLands.Remove(p);
        }
    }
    public void ScoutLand(Province p)
    {
        RemoveScoutedLand(p);
        scoutedLands.Add(p.provinceName, new Province(p));
    }
    public void RemoveScoutedLand(Province p)
    {
        if(scoutedLands.ContainsKey(p.provinceName)) scoutedLands.Remove(p.provinceName);
    }
    public delegate void ProvinceAction(Province p);

    public Dictionary<string, Province> GetKnownLands()
    {
        Dictionary<string, Province> knownLands = new Dictionary<string, Province>();
        for(int i = 0; i < ownedLands.Count; i++)
        {
            knownLands.Add(ownedLands[i].provinceName, ownedLands[i]);
        }
        for (int i = 0; i < scoutedLands.Count; i++)
        {
            knownLands.Add(scoutedLands.Keys.ElementAt(i), scoutedLands.Values.ElementAt(i));
        }
        return knownLands;
    }
    public Province[] GetOwnedLands()
    {
        return ownedLands.ToArray(); ;
    }
    public string[] GetActiveScouts()
    {
        return scouting.ToArray();
    }
    public string[] GetActiveAttacks()
    {
        return attacking.ToArray();
    }
}
