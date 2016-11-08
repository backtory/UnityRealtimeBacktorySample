namespace Sdk.Core {
	public abstract class BacktoryMatchMessage : BacktoryMessage {

		public static string CHAT = ".ChatMessage";
		public static string MASTER = ".MasterMessage";
		public static string DIRECT = ".DirectMessage";
		public static string CHALLENGE = ".ChallengeMessage";
		public static string WEBHOOK_ERROR = ".WebhookErrorMessage";
		public static string JOINED_WEBHOOK = ".JoinedWebhookMessage";
		public static string STARTED_WEBHOOK = ".StartedWebhookMessage";
		public static string CHALLENGE_ENDED = ".ChallengeEndedMessage";
		public static string CHALLENGE_JOINED = ".ChallengeJoinedMessage";
		public static string CHALLENGE_STARTED = ".ChallengeStartedMessage";
		public static string CHALLENGE_DISCONNECT = ".ChallengeDisconnectedMessage";
	
	}
}
