Running the scene will run the updated method with a sample set of cars.
The console log shows the list of cars, by their ID, before and after the method has been run,
Collisions are calculated as 
   thisRacer.Id % _crashMod == 0 && collisionRacer.Id % _crashMod == 0;
Where _crashMod = 2;