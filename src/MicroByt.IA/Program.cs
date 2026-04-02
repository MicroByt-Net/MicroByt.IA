#if SAMPLES
using MicroByt.IA.API.Samples;

await SelectSkillsSample.RunTest1();
#else
return; // define SAMPLES to run sample scenarios
#endif