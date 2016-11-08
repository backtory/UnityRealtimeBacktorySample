using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Sdk.Core.Models.Connectivity.Challenge;
using Sdk.Core.Models.Connectivity.Matchmaking;
using Sdk.Core.Models;
using Assets.Backtory.core;

namespace Sdk.Unity {
	public class BacktoryRealtimeUnityApi : BacktoryConnectivityUnityApiWrapper {
		
		internal static String X_BACKTORY_CONNECTIVITY_ID;
		
		private static BacktoryRealtimeUnityApi backtoryApi;
		private static UnityPlatform unityPlatform;
		
		private static Dictionary<String, BacktoryRealtimeMatchUnityApi> matchApiMap = new Dictionary<String, BacktoryRealtimeMatchUnityApi>();
		
		private BacktoryRealtimeUnityApi(String xBacktoryConnectivityId/*, Context baseContext*/) {
			X_BACKTORY_CONNECTIVITY_ID = xBacktoryConnectivityId;
			unityPlatform = new UnityPlatform();
			backtoryConnectivityApi = new InnerBacktoryConnectivityUnityApi(unityPlatform, this);
		}
		
		public static BacktoryRealtimeMatchUnityApi GetMatchApi(String matchId) {
			return matchApiMap[matchId];
		}
		
		private void generateRealtimeMatch(String address, String matchId) {
			BacktoryRealtimeMatchUnityApi matchApi = new BacktoryRealtimeMatchUnityApi (address, matchId, X_BACKTORY_CONNECTIVITY_ID);
			matchApiMap.Add(matchId, matchApi);
		}

//		internal class InnerBacktoryMatchUnityApi : BacktoryMatchUnityApi {
//			internal InnerBacktoryMatchUnityApi(String address, String matchId) : base(address, matchId) {
//			}
//
//			override
//			internal String GetConnectivityId() {
//				return X_BACKTORY_CONNECTIVITY_ID;
//			}
//		}

		public static void Initialize(String xBacktoryConnectivityId/*, Context baseContext*/) {
			// Alireza
            //if(UnityThreadHelper.Dispatcher == null) {
			//	throw new Exception("Initialization should run on main thread");
			//}
			if (backtoryApi == null)
				backtoryApi = new BacktoryRealtimeUnityApi(xBacktoryConnectivityId/*, baseContext*/);
		}
		
		public static BacktoryRealtimeUnityApi Instance() {
			if (backtoryApi == null)
				throw new Exception("The Backtory API has not yet been initialized");
			
			return backtoryApi;
		}
		
//		public void connectAsync(String username) {
//			// TODO threading
//			new AsyncTask<Void, Void, Void>() {
//				@Override
//				protected Void doInBackground(Void... params) {
//					connect(username);
//					return null;
//				}
//			}.execute();
//		}
		
		public BacktoryResponse<ConnectResponse> Connect() {
//			if (username.Equals("ali")) {
//				UnityWebSocket.token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNTcyMGIwMTZlNGIwYmYxMWY5MGNkZWU2IiwiaXNfZ3Vlc3QiOmZhbHNlLCJ1c2VyX25hbWUiOiJhbGkiLCJzY29wZSI6WyJjbGllbnQiXSwiZXhwIjoxNjY4OTIwNzk2LCJqdGkiOiIwNDRhYmE0ZC01MzA2LTQzYWQtYmMwNi0wMTIyODc4MGU3YjgiLCJjbGllbnRfaWQiOiI1NmYzNmE5ZmU0YjA2OGI1YWEyMWUzMzUifQ.ARNyyPayf3tm_luo7BxZ1vKeUYcvXUlrPA8xslN-HPzM8TwLEctVmOQp9aRWHTaBe5b77Is6cPbzfjGbc0E9diMDG3__4uu8Fv-o13UrfOtvxLMQIIhQRphKdUSx2z8gsLFknSO8u98PWfna0m3_yDxUtd6NNv81uiL4uYoSDpY2gDYlJ25Rg4eo73-mIQ7rdxesW4TpwzrXO2a2QLaHnRuJfTVYfv0XY6PsaH24QdkUN687NqukjN_iQCSmRG_XA88Vf3Cg9BfdcduHFCP4NRbbtjPoYDEbVgX03tUuD_L7nz54UC8PnVm5M26kd9-ZxUAw_nAk4WY8IZONnxSoNA";
//			} else if (username.Equals("javad")) {
//				UnityWebSocket.token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNTcyMGIwMjNlNGIwYmYxMWY5MGNkZWU5IiwiaXNfZ3Vlc3QiOmZhbHNlLCJ1c2VyX25hbWUiOiJqYXZhZCIsInNjb3BlIjpbImNsaWVudCJdLCJleHAiOjE2Njg5MjA4MjUsImp0aSI6ImUwOTEzMzU1LTU1YzYtNDM2ZC1iMTk3LTAyOGY5YmNmNzQyYiIsImNsaWVudF9pZCI6IjU2ZjM2YTlmZTRiMDY4YjVhYTIxZTMzNSJ9.wbdgW0GCSzESsfR1A2QTk87fSpBD5bkP97HRdqcdrTQx6D0s0oRhtJ8jLEz9XaAA0E97oXr_wHjnL5RIbQ5IuBxysMjdtNzx4lJZ_FISRZOYW_CLbEzmGRRZR9c72tYAOhe9BDx64YX-kxaJegUx-3XFayMLjBoF_3i7x8GWCmGgmF4V9qSJjdxNTcYyK0tBsYzLAVYcS0Q29DOe6IqRYyJYZbDkrAN3DvzyOZIKCChdEEe2Vn_KOoLxxHup-xttrp4VQf_yTeVzYa3RL8Mf25y0S_wqo24tnGVAvuSDBEFfnszr-njG5bRp_Ix4EgUH8P9gn28BhwysoTRBxLvCHA";
//			} else if (username.Equals("farib")) {
//				UnityWebSocket.token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNTcyMGIwMWVlNGIwYmYxMWY5MGNkZWU4IiwiaXNfZ3Vlc3QiOmZhbHNlLCJ1c2VyX25hbWUiOiJmYXJpYiIsInNjb3BlIjpbImNsaWVudCJdLCJleHAiOjE2Njg5MjA4MzQsImp0aSI6ImM3NDVhNWNkLTMxNmItNDkxNi05MWI2LTE5YzllNjI4OWFhNCIsImNsaWVudF9pZCI6IjU2ZjM2YTlmZTRiMDY4YjVhYTIxZTMzNSJ9.FRqQMYYqWz3pI7C_Isjeo12G1RUpVvr7KfIUV4lpDErFzIBHDD3TJbqSgyuHDJD0TZMFCt1e0UWdXGvV22EtkZQjP4X3skfk9kpFTM6WIHe8ePLvysXcdQPuLOMtWvNALxp5pHoqbEepZlyyiCKG2iwwmJ60iZT0wEJxd7m5qR-eWyB2kzSFkbahjDpdbP9EYgsk81J8LpDY9xYM_8ZC-ourF2HuH4WuBOxQlhRqrjZ2OdRLxfHipi0D8ImxE54TaqOW4U7hMX1wWiyu1b6J5Dz4joKCMyfe2rATX7Mhs_XHFlvQQvspyQICN2Riw7-oDUBI8qxeSSmv3q_t31zNjw";
//			} else if (username.Equals("mamad")) {
//				UnityWebSocket.token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNTcyMGIwMWJlNGIwYmYxMWY5MGNkZWU3IiwiaXNfZ3Vlc3QiOmZhbHNlLCJ1c2VyX25hbWUiOiJtYW1hZCIsInNjb3BlIjpbImNsaWVudCJdLCJleHAiOjE2Njg5MjA4NDYsImp0aSI6IjJlMjc5YTEzLWE4ZGYtNGU3NC1iYWQxLTFmZmYwMjA1MDAwMSIsImNsaWVudF9pZCI6IjU2ZjM2YTlmZTRiMDY4YjVhYTIxZTMzNSJ9.y3jgDgyZ84NzOivfONaAJi_DyWcajPcx1mL300WY7lHD8rMOJ59nT8f2d7X1n4hqsMjnAJiu4UTdn9QF5mQ5K5c0cAw0BC6CFjiIIvokT166lk3Ih4MPmKBOnfp2gUJ_xeOxuylCqbTya1Cyt6FccGS8di3S1nGtAYAyJ6RzjTQ_LCR46tQkp6EvCxRXw_Lu8kwzbcYWxNCiQoiD6FcSziZTQf2RTncdzsAx5qgA2Lf6XMdKCKeKbWdUa7-vpK9692j6pZ_HlGHbgwYB3BX_4h_kPNRyGqXyl3Z8t4g83ptINXXonbEIkFlfbhVdECSmAK-DzmT0YMp1VprpMKL3Qg";
//			} else {
//				UnityWebSocket.token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiNTcyNDY0MDFlNGIwMDA5NTdiNTZlNGM2IiwiaXNfZ3Vlc3QiOmZhbHNlLCJ1c2VyX25hbWUiOiJ1c2VyMSIsInNjb3BlIjpbImNsaWVudCJdLCJleHAiOjE2Njg5MjA4NzUsImp0aSI6ImE3Yzc5MDYyLTJhYjgtNGU0ZS04ZjgwLTU2NGM0ZTBhM2U1YyIsImNsaWVudF9pZCI6IjU2ZjM2YTlmZTRiMDY4YjVhYTIxZTMzNSJ9.sHoT6VTBzZqVH5dFW8OgIQoJVowfTEP1w9o5V5It7hWK-DmdSSk_tPoxrxuNNPc42BX1XOan7qQBqcIp2-DvU-c6WCrfeEpKanpSoYJr8cF8D635KQOFNjxSg08YTI32rHJKYqC2yf5msPV-8D9e6P0srFPnJLITGCwcm_6JeNuLdFK2lxDqJTa8tfRP3y7xcp_j4CnxTCs6kAYPYf97tde41rXKWQs2ftlgUp32jfouURZk1EmPXlBagsAy739H9vN-wZVwGuyMil_NX-oBmCNDyatAErzLWig9TuCmETW6vi8gUWdHhOU2NYqvmDsdj9TuCo3cIv8ZrX-FjbwJ6g";
//			}
			
			return backtoryConnectivityApi.Connect();
		}

		internal class InnerBacktoryConnectivityUnityApi : BacktoryConnectivityUnityApi {

			BacktoryRealtimeUnityApi outClass;

			internal InnerBacktoryConnectivityUnityApi(UnityPlatform unityPlatform, BacktoryRealtimeUnityApi unityApi) : base(unityPlatform) {
				outClass = unityApi;
			}

			override
			internal String GetConnectivityId() {
				return X_BACKTORY_CONNECTIVITY_ID;
			}
			
			override
			internal void GenerateRealtimeMatch(MatchFoundMessage message) {
				outClass.generateRealtimeMatch(message.Address, message.MatchId);
			}
			
			override
			internal void GenerateRealtimeMatch(ChallengeReadyMessage message) {
				outClass.generateRealtimeMatch(message.Address, message.MatchId);
			}
		}		
	}
}
