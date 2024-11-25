public class Vehicle {
    public int passedWaypoints;
    public string name;
    public bool hasFinished;
    public bool isPlayer;

    public Vehicle(int passedWaypoints,string name,bool hasFinished, bool isPlayer){
        this.passedWaypoints = passedWaypoints;
        this.name = name;
        this.hasFinished = hasFinished;
        this.hasFinished = isPlayer;
    }
}
