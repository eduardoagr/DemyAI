﻿using User = Firebase.Auth.User;

namespace DemyAI.Services;

public class AuthenticationService(FirebaseAuthClient firebaseAuthClient, IAppService appService) : IAuthenticationService {

    public async Task<User?> GetLoggedInUser() {

        try {

            return firebaseAuthClient.User;

        } catch(Exception e) {

            await appService.DisplayAlert("error", e.Message, "OK");
            return null;
        }
    }

    public async Task<User?> LoginWithEmailAndPassword(string email, string password) {

        try {

            var user = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            return user.User;

        } catch(Exception e) {

            await appService.DisplayAlert("error", e.Message, "OK");
            return null;

        }

    }

    public Task<string> LoginWithStudentId(string id) {
        throw new NotImplementedException();
    }

    public async Task<User?> RegisterWithEmailAndPassword(string email, string password, string name) {

        try {

            var user = await firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, name);
            return user.User;

        } catch(Exception e) {

            await appService.DisplayAlert("error", e.Message, "OK");
            return null;
        }

    }

    public void SignOut() {
        firebaseAuthClient.SignOut();
    }
}
