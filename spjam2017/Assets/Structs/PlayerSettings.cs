
using Identifiers;

namespace Structs {
	public class PlayerSettings {

		public PlayerID id;
		public bool isPlaying;
		public TeamID team;

		public PlayerSettings(PlayerID id, bool isPlaying, TeamID team) {
			this.id = id;
			this.isPlaying = isPlaying;
			this.team = team;
		}
	}
}