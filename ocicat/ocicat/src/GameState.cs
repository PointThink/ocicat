namespace ocicat;

public class GameState
{
	public virtual void OnEnter() {}
	public virtual void OnLeave() {}
	
	public virtual void Update() {}
	public virtual void Tick() {}
	public virtual void Draw() {}
}