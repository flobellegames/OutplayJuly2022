using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace com.flobelle
{
    //Consider a hypothetical racing game where hundreds of cars race on a field.
    //The updateRacers method shown below updates the cars and eliminates the ones that collide. 

    //void UpdateRacersOriginal(float deltaTimeS, List<Racer> racers)
    //{
    //    List<Racer> racersNeedingRemoved = new List<Racer>();
    //    racersNeedingRemoved.Clear();

    //    // Updates the racers that are alive
    //    int racerIndex = 0;
    //    for (racerIndex = 1; racerIndex <= 1000; racerIndex++)
    //    {
    //        if (racerIndex <= racers.Count)
    //        {
    //            if (racers[racerIndex - 1].IsAlive())
    //            {
    //                //Racer update takes milliseconds
    //                racers[racerIndex - 1].Update(deltaTimeS * 1000.0f);
    //            }
    //        }
    //    }
    //    // Collides
    //    for (int racerIndex1 = 0; racerIndex1 < racers.Count; racerIndex1++)
    //    {
    //        for (int racerIndex2 = 0; racerIndex2 < racers.Count; racerIndex2++)
    //        {
    //            Racer racer1 = racers[racerIndex1];
    //            Racer racer2 = racers[racerIndex2];
    //            if (racerIndex1 != racerIndex2)
    //            {
    //                if (racer1.IsCollidable() && racer2.IsCollidable() && racer1.CollidesWith(racer2))
    //                {
    //                    OnRacerExplodes(racer1);
    //                    racersNeedingRemoved.Add(racer1);
    //                    racersNeedingRemoved.Add(racer2);
    //                }
    //            }
    //        }
    //    }
    //    // Gets the racers that are still alive
    //    List<Racer> newRacerList = new List<Racer>();
    //    for (racerIndex = 0; racerIndex != racers.Count; racerIndex++)
    //    {
    //        // check if this racer must be removed
    //        if (racersNeedingRemoved.IndexOf(racers[racerIndex]) < 0)
    //        {
    //            newRacerList.Add(racers[racerIndex]);
    //        }
    //    }
    //    // Get rid of all the exploded racers
    //    for (racerIndex = 0; racerIndex != racersNeedingRemoved.Count; racerIndex++)
    //    {
    //        int foundRacerIndex = racers.IndexOf(racersNeedingRemoved[racerIndex]);
    //        if (foundRacerIndex >= 0) // Check we've not removed this already!
    //        {
    //            racersNeedingRemoved[racerIndex].Destroy();
    //            racers.Remove(racersNeedingRemoved[racerIndex]);
    //        }
    //    }
    //    // Builds the list of remaining racers
    //    racers.Clear();
    //    for (racerIndex = 0; racerIndex < newRacerList.Count; racerIndex++)
    //    {
    //        racers.Add(newRacerList[racerIndex]);
    //    }

    //    for (racerIndex = 0; racerIndex < newRacerList.Count; racerIndex++)
    //    {
    //        newRacerList.RemoveAt(0);
    //    }
    //}
    // 	Rewrite the method to improve its readability and performance without changing its behaviour
    // 	Describe further changes you would make if you could change its behaviour. Discuss your reasoning for making these changes.


    public class QuestionOne : MonoBehaviour
    {
        List<Racer> _racers = new List<Racer>();

        // Start is called before the first frame update
        void Start()
        {
            _racers.Add(new Racer(0));
            _racers.Add(new Racer(1));
            _racers.Add(new Racer(2));
            _racers.Add(new Racer(3));
            _racers.Add(new Racer(4));
            _racers.Add(new Racer(5));
            _racers.Add(new Racer(6));
            _racers.Add(new Racer(7));
            _racers.Add(new Racer(8));
            _racers.Add(new Racer(9));

            Debug.Log($"Racers Active before collisions {string.Join(",", _racers.Select(r => r.Id))}");
            _racers = UpdateRacers(1f, _racers);
            Debug.Log($"Racers Active after collisions {string.Join(",", _racers.Select(r => r.Id))}");
        }

 
        List<Racer> UpdateRacers(float deltaTimeS, List<Racer> racers)
        {
            // Updates the racers that are alive
            for (int racerIndex = 0; racerIndex < racers.Count; racerIndex++)
            {
                //Racer update takes milliseconds // VC - if this is taking milliseconds to complete this should be shifted off to an async call, else we're having to wait X milliseconds multiplied by the number of racers
                racers[racerIndex].Update(deltaTimeS * 1000.0f); // VC - assumption made for this exercise that this has to be run for each Racer before we check for collisions
            }

            // Collides
            for (int racerIndex = 0; racerIndex < racers.Count; racerIndex++)
            {
                bool racerCrashes = false;
                Racer racer1 = racers[racerIndex];

                for (int otherRacersIndex = racers.Count - 1; otherRacersIndex > racerIndex; otherRacersIndex--)
                {
                    Racer otherRacer = racers[otherRacersIndex];

                    if (racer1.IsCollidable() && otherRacer.IsCollidable() && racer1.CollidesWith(otherRacer))
                    {
                        racerCrashes = true;
                        OnRacerExplodes(otherRacer);
                        otherRacer.Destroy();
                        racers.RemoveAt(otherRacersIndex);
                    }
                }

                if (racerCrashes)
                {
                    OnRacerExplodes(racer1);
                    racer1.Destroy();
                    racers.RemoveAt(racerIndex);
                }
            }

            return racers;
        }

        void OnRacerExplodes(Racer racer)
        {
        }
    }
    public class Racer
    {
        const int _crashMod = 2;
        public int Id = 0;

        public Racer(int id)
        {
            Id = id;
        }

        public void Update(float t)
        {
        }

        public bool IsAlive()
        {
            return true;
        }

        public bool IsCollidable()
        {
            return true;
        }

        public bool CollidesWith(Racer racer)
        {
            return Id % _crashMod == 0 && racer.Id % _crashMod == 0;
        }

        public void Destroy()
        {
        }
    }
}