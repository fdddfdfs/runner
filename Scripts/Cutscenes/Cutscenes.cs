public class Cutscenes
{
    private const string BaseStartCutsceneResourceName = "BaseStartCutscene";

    private Cutscene _cutscene;
    
    public Cutscenes(Run run, Fade fade)
    {
        var baseStartCutscene = ResourcesLoader.InstantiateLoadComponent<BaseStartCutscene>(
            BaseStartCutsceneResourceName);

        baseStartCutscene.Init(run, fade);

        _cutscene = baseStartCutscene;
        _cutscene.SetCutscene();
    }

    public void PlayCutscene()
    {
        _cutscene.PlayCutscene();
    }
}