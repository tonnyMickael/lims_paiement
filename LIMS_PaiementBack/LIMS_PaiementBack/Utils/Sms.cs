// using FirebaseAdmin;
// using FirebaseAdmin.Messaging;
// using Google.Apis.Auth.OAuth2;
// namespace LIMS_PaiementBack.Utils
// {
//     public class Sms
//     {
//        private readonly FirebaseMessaging _messaging;

//         public Sms()
//         {
//             FirebaseApp.Create(new AppOptions()
//             {
//                 Credential = GoogleCredential.FromFile("chemin/vers/ton-fichier-serviceAccountKey.json")
//             });
//             _messaging = FirebaseMessaging.DefaultInstance;
//         }

//         public async Task SendNotificationAsync(string token, string title, string body)
//         {
//             var message = new Message()
//             {
//                 Token = token,
//                 Notification = new Notification()
//                 {
//                     Title = title,
//                     Body = body
//                 }
//             };

//             await _messaging.SendAsync(message);
//         }
//     }
// }