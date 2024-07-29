# Observer Pattern

The [observer pattern](https://en.wikipedia.org/wiki/Observer_pattern) is an essential pattern in modern programming, most applications implement this in some form, regardless of language.

The idea behind observer, is we want to be notified when things happen in a manor that doesn't expose data and can be fully autonomous


The observer pattern addresses the following problems: [(Taken from Wikipedia)](https://en.wikipedia.org/wiki/Observer_pattern)
* A one-to-many dependency between objects should be defined without making the objects tightly coupled.
* When one object changes state, an open-ended number of dependent objects should be updated automatically.
* An object can notify multiple other objects.

# Example

Let's say whenever an enemy detects the player we MAY want
to notify different systems... For instance, we may want to 
notify an ability that gives the player increased move speed, 
We may want to play a UI sound, We may want to spawn a particle/object etc.
Let's also say this is a game where multiple players can exist
at once (so the example makes more sense)

First create the player class
```csharp
public class Player : MonoBehaviour{
    
    //Actions, Delegates and Events are all similar ways of doing this with unique quirks
    public Action onDetected;
    
    public void OnDetected()
    {
        // ?. means do something, IF onDetected IS NOT NULL.
        //Anything that is subscribed (+=) to this function will be notified
        onDetected?.invoke();
    }
}
```
Then create the SpeedBuff class
```csharp
public class SpeedBuff : MonoBehaviour{
    Player player;
    private Awake()
    {
        //Assume the player is connected to the same object
        player = GetComponent<Player>();
        //Subscribe to the event so we are notified when it happens
        player.onDetected += AddSpeedBuff;
    }
    
    private void OnDestroy()
    {
        //Unsubscribe to make sure memory isn't persisting
        player.onDetected -= AddSpeedBuff;
    }
    
    public void AddSpeedBuff()
    {
        if(CAN SPEED BUFF LOGIC)
        {
            StartCoroutine(HandleSpeedBuff);
        }
    }
    
    // (Additional partial code, not related to observer)
    private IEnumerator HandleSpeedBuff()
    {
        ADD BUFF TO palyer
        HANDLE SPEED BUFF DURATION
        ON COMPLETE RemoveSpeedBuff()
    }
    
    private void RemoveSpeedBuff()
    {
        REMOVE BUFF FROM player
    }
}
```
Now finally in enemy, if we detect a player, let's notify it. 
```csharp
public class Enemy : MonoBehaviour
{
    void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.foward, out RaycastHit hit, 50))
        {
            if(hit.TryGetComponent(out Player player))
            {
                //Call the OnDetected funtion (Which invokes the action)
                player.OnDetected();
            }
        }
    }
}
```