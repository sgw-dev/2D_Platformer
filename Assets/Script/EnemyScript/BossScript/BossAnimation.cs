public class BossAnimation
{
    /*
     * By: Parker Allen
     * Version: 1.0
     * 
     */

    public float[] angles;      //array of angles skeleton to move to
    public float time;          //amount of time animation takes

    //constructors
    public BossAnimation(float[] ang, float t)
    {
        angles = ang;
        time = t;
    }

    public BossAnimation(float t)
    {
        angles = null;
        time = t;
    }
}
