using Android.App;
using Android.Widget;
using Android.OS;
using Minimize.Api.Client;


namespace TourStops.droid
{
    [Activity(Label = "TourStops.droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private int currentProgress = 0;
        private static ApiClient client;
        private static Minimize.Api.Client.Models.SignupRequest signup;
        private static Minimize.Api.Client.Models.SignupResponse signupResponse;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button tourStop1Button = FindViewById<Button>(Resource.Id.tourStop1Button);
            Button tourStop2Button = FindViewById<Button>(Resource.Id.tourStop2Button);

            tourStop1Button.Click += delegate
            {
                CallNumber(tourStop1Button.Text);
            };

            tourStop2Button.Click += delegate
            {
                CallNumber(tourStop2Button.Text);
            };

            SetupClickButton();
            SetupCheckboxListener();

            ProgressBar progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            Button progressIncrementingButton = FindViewById<Button>(Resource.Id.progressIncrementingButton);
            progressIncrementingButton.Click += delegate
            {
                IncrementProgressBar(progressBar);
            };

            RadioButton radio_Ferrari = FindViewById<RadioButton>(Resource.Id.radioFerrari); 
            RadioButton radio_Mercedes = FindViewById<RadioButton>(Resource.Id.radioMercedes); 
            RadioButton radio_Lambo = FindViewById<RadioButton>(Resource.Id.radioLamborghini); 
            RadioButton radio_Audi = FindViewById<RadioButton>(Resource.Id.radioAudi); 
            radio_Ferrari.Click += OnClickRadioButton; 
            radio_Mercedes.Click += OnClickRadioButton; 
            radio_Lambo.Click += OnClickRadioButton; 
            radio_Audi.Click += OnClickRadioButton; 

        }

        private void OnClickRadioButton(object sender, System.EventArgs e)
        {
            RadioButton cars = (RadioButton)sender;
            Toast.MakeText(this, cars.Text, ToastLength.Short).Show();
        }

        private void SetupCheckboxListener()
        {
            CheckBox checkMe = FindViewById<CheckBox>(Resource.Id.CheckboxTest);
            checkMe.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => {
                CheckBox check = (CheckBox)sender;
                if (check.Checked)
                {
                    check.Text = "Checkbox has been checked";
                }
                else
                {
                    check.Text = "Checkbox has not been checked";
                }
            };
        }

        private void IncrementProgressBar(ProgressBar progressBar)
        {
            if(this.currentProgress >= 100)
            {
                string toastText = "You have reached 100%";
                Toast.MakeText(this, toastText, ToastLength.Short).Show();
                progressBar.Progress = 5;
                this.currentProgress = 5;
                return;
            };
            progressBar.Progress = this.currentProgress += 10;
        }

        private async void SetupClickButton()
        {
            Button button = FindViewById<Button>(Resource.Id.ClickyButtonTextChanger);
            button.Click += async delegate 
            {
                client = new ApiClient();
                signup  = new Minimize.Api.Client.Models.SignupRequest
                {
                    email = "pmm4654@gmail.com",
                    first_name = "integration",
                    last_name = "tests",
                    password = "Password123",
                    password_confirmation = "Password123"
                };
                signupResponse = client.Signup(signup).Result;
                Toast.MakeText(this, "You clicked me!", ToastLength.Short).Show();
            };
        }

        private void CallNumber(string phoneNumber)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("Call " + phoneNumber);

            callDialog.SetPositiveButton("Call", delegate {
                var callIntent = new Android.Content.Intent(Android.Content.Intent.ActionCall);
                callIntent.SetData(Android.Net.Uri.Parse("tel:" + phoneNumber));
                StartActivity(callIntent);
            });

            callDialog.SetNeutralButton("Cancel", delegate { });

            callDialog.Show();
        }
    }
}

