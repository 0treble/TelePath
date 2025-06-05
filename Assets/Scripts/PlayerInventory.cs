using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCollectibles { get; private set; }
    private MazeGenerator mazeGenerator;

    public UnityEvent<PlayerInventory> OnPickup;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private void Start()
    {
        mazeGenerator = Object.FindFirstObjectByType<MazeGenerator>();
    }

    public void CollectiblesCollected(Vector3 playerPosition)
    {
        NumberOfCollectibles++;
        OnPickup.Invoke(this);

        // Play pickup sound
        PlaySound(pickupSound);

        mazeGenerator.RegenerateMaze(playerPosition);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
