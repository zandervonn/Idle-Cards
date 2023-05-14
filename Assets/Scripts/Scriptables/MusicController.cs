//using UnityEngine;
//using UnityFluidSynth;

//public class MusicController : MonoBehaviour
//{
//    public int maxMana = 100;
//    public int currentMana;
//    public float minTempo = 60;
//    public float maxTempo = 180;
//    public FluidSynthPlayer fluidSynthPlayer;
//    public int pianoProgram = 0; // General MIDI program number for Acoustic Grand Piano

//    void Start()
//    {
//        fluidSynthPlayer = GetComponent<FluidSynthPlayer>();

//        // Load a SoundFont file (.sf2) containing instrument sounds
//        fluidSynthPlayer.LoadSoundFont("path/to/your/soundfont.sf2");

//        // Set the MIDI program (instrument) for channel 0 to piano
//        fluidSynthPlayer.MidiProgramChange(0, pianoProgram);

//        // Start playing a simple repeating melody
//        InvokeRepeating("PlayMelody", 0, 1);
//    }

//    void PlayMelody()
//    {
//        float tempo = Mathf.Lerp(minTempo, maxTempo, 1 - (float)currentMana / maxMana);
//        fluidSynthPlayer.MidiTempo = (int)tempo;

//        // Play a simple C major arpeggio
//        int[] notes = { 60, 64, 67, 72 }; // C4, E4, G4, C5
//        int noteDuration = 1000000 / (int)tempo / 4; // Quarter note duration in microseconds

//        for (int i = 0; i < notes.Length; i++)
//        {
//            fluidSynthPlayer.MidiNoteOn(0, notes[i], 100);
//            fluidSynthPlayer.MidiNoteOff(0, notes[i], noteDuration * (i + 1));
//        }
//    }

//    public void GameReset()
//    {
//        // Start a crescendo over 2 seconds
//        StartCoroutine(Crescendo(2));

//        // Reset mana and other game variables here
//    }

//    IEnumerator Crescendo(float duration)
//    {
//        float startVolume = fluidSynthPlayer.MidiChannelVolume[0];
//        float endVolume = 127;

//        for (float t = 0; t < duration; t += Time.deltaTime)
//        {
//            float volume = Mathf.Lerp(startVolume, endVolume, t / duration);
//            fluidSynthPlayer.MidiChannelVolume[0] = (int)volume;
//            yield return null;
//        }

//        // After the crescendo, set the volume back to the starting value
//        fluidSynthPlayer.MidiChannelVolume[0] = (int)startVolume;
//    }
//}