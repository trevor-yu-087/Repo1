using System;
using System.Collections.Generic;
using System.IO;

using static System.Console;

namespace HockeyDraft
{
    class DraftSelect       //Each person holds a name, number of forwards/goalies, position in draft and list of Players
    {
        string name;
        int numFG;
        int numD;
        int draftPosition;  //Indexed starting at 1
        List< Player > selections;
        
        public string Name {get {return name;} }
        public int DraftPosition {get {return draftPosition;} }
        public int FG {get {return numFG;} }
        public int D { get {return numD;} }
        public List<Player> Selections { get {return selections;} }
        
        public DraftSelect( string name, int draftPosition )
        {
            this.name = name;
            this.draftPosition = draftPosition;
            selections = new List< Player >();
            numD = 0;
            numFG = 0;
        }
        
        public override string ToString()
        {
            return $"{name} ({draftPosition})";
        }
        
        public void AddPlayer( Player selection )
        {
            selections.Add(selection);
            WriteLine($"{this.name} selects {selection}");
            if(selection.Position == "D") numD ++;
            else if(selection.Position == "G" || selection.Position == "F") numFG ++;
        }
        
    }
    

    
    class Player
    {
        public int nhlID;
        string lastName;
        string firstName;
        string position;
        string FDG;
        string team;
        bool taken;
        int overallDrafted;
        int roundDrafted;
        DraftSelect draftedBy;
        
        public int NHLID { get {return nhlID;} }
        public string LastName { get {return lastName;} }
        public string FirstName { get {return firstName;} }
        public string Name{ get {return string.Format("{0} {1}", firstName, lastName);} }
        public string Position{ get {return FDG;} }
        public string Team{ get {return team;} }
        public bool IsTaken{ get {return taken;} }
        public int Overall { get {return overallDrafted;} }
        public int Round { get {return roundDrafted;} }
        public DraftSelect DraftedBy { get {return draftedBy;} } 
        
        public Player() {}
        
        public Player( string fileLine )       //Constructor for file line
        {
            string[] fields = fileLine.Split( ',' );
            
            this.nhlID = int.Parse(fields[0]);
            this.lastName = fields[1];
            this.firstName = fields[2];
            this.position = fields[3];
            
            if( this.position == "D" ) FDG = "D";    //See if their position is forward or goalie
            else FDG = "F";
            
            if( fields[4].Length > 3 )  //if the team field lists multiple teams
                this.team = $"{fields[4].Substring(0,3)} ({fields[4].Substring(4)})";   //OTT (VAN/PHX)
            else this.team = fields[4];
            
            this.taken = false;
        }
        
        public void Drafted(DraftSelect myTurn, int currentRound, int currentPick)
        {
            this.taken = true;
            this.roundDrafted = currentRound;
            this.overallDrafted = currentPick;
            this.draftedBy = myTurn;
        }
        
        public override string ToString( )
        {
            if(taken) return $"{Name} was selected by {draftedBy} in round {roundDrafted}";
            return $"#{NHLID}  {Name}  {team}";
        }
    }
    
    class DraftManager
    {
        DraftSelect[] participants;
        int totalRounds;
        int currentRound;
        int currentPick;
        int numDef;
        
        public int CurrentRound {get {return currentRound;} }
        public int CurrentPick {get {return currentPick;} }
        public int TotalRounds {get {return totalRounds;} }
        
        public DraftManager( DraftSelect[] participants, int totalRounds, int numDef)
        {
            this.participants = participants;
            this.totalRounds = totalRounds;
            this.numDef = numDef;
            currentRound = 1;
            currentPick = 1;
        }
        
        //Take file, turn into nice file with storage
        public static List<Player> GenerateNewPlayerList( string fileName )
        {
            List< Player > playerList = new List< Player >();
            
            using( FileStream stream = new FileStream( fileName, FileMode.Open, FileAccess.Read ) )
            using( StreamReader reader = new StreamReader( stream ) )
            {
                if( fileName == null ) return null;
                if( File.Exists( fileName ) == false ) return null;
                
                string header = reader.ReadLine();
                while( reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();
                    Player player = new Player(line);
                    playerList.Add( player );
                }
            }
            return playerList;
        }
        
        public static void WritePlayerList( List<Player> playerList)
        {
            foreach( Player player in playerList)
                WriteLine( player );
        }
        
        void SelectPlayer( List< Player > playerList, DraftSelect myTurn )  //Important method!!
        {
            string mode;
            
            WriteLine( "\n\nSearch playerList for player name. Type 'f' for first name search, 'l' for last name search, 't' for team search and '#' to enter selection using nhlID.");
            Write( "Select search mode: " );
            mode = ReadLine();

            while( mode != "#" )
            {
                if( mode == "f" )
                    DraftManager.FirstNameSearch(playerList);
                else if( mode == "l" )
                    DraftManager.LastNameSearch(playerList);
                else if( mode == "t" )
                    DraftManager.TeamSearch(playerList);
                
                WriteLine( "\n\nSearch playerList for player name. Type 'f' for first name search, 'l' for last name search, 't' for team search and '#' to enter selection using nhlID.");
                Write( "Select search mode: " );
                mode = ReadLine();
            }
            NHLIDSearch(playerList, myTurn);
        }
        
        static void LastNameSearch( List<Player> playerList)
        {
            Clear();
            Write( "Enter last name search (case sensitive): " );
            string search = ReadLine();
            for(int i = 0; i < playerList.Count; i ++)
            {   
                if( playerList[i].LastName.Contains(search) )
                {
                    WriteLine( playerList[i] );
                }
            }
        }
        
        static void FirstNameSearch( List<Player> playerList)
        {
            Clear();
            Write( "Enter first name search (case sensitive): " );
            string search = ReadLine();
            for(int i = 0; i < playerList.Count; i ++)
            {   
                if( playerList[i].FirstName.Contains(search) )
                {
                    WriteLine( playerList[i] );
                }
            }
        }
        
        static void TeamSearch( List<Player> playerList)
        {
            Clear();
            Write( "Enter team search. Teams are all-caps 3 character codes (eg VAN or L.A): " );
            string search = ReadLine();
            for(int i = 0; i < playerList.Count; i ++)
            {   
                if( playerList[i].Team.Contains(search) )
                {
                    WriteLine( playerList[i] );
                }
            }
        }
        
        void NHLIDSearch( List<Player> playerList, DraftSelect myTurn)
        {
            bool foundPlayer = false;
            
            while( foundPlayer == false)
            {
                Write("Enter NHL ID # for the player you wish to select: #");
                int search = int.Parse(ReadLine());
                for( int i = 0; i < playerList.Count; i ++ )
                {
                    if( playerList[i].NHLID == search )
                    {
                        if( playerList[i].IsTaken == false )
                        {
                            myTurn.AddPlayer(playerList[i]);
                            playerList[i].Drafted(myTurn, currentRound, currentPick);         //Sets the info on player to drafted state
                            foundPlayer = true;
                            return;
                        }
                        else WriteLine( $"{playerList[i].Name} was taken by {playerList[i].DraftedBy} in Round {playerList[i].RoundDrafted}");
                    }
                }
            }
        }
        
        void DisplaySelections( DraftSelect myTurn )
        {
            WriteLine( $"{myTurn.Name}'s selections: " );
            int i = 0;
            foreach( Player player in myTurn.Selections )
            {
                WriteLine( $"Round{i+1}: {player.Name} {player.Team} ({player.Position})" );
                i ++;
            }
            while( i < totalRounds )
            {
                WriteLine( $"Round{i+1}: " );
                i ++;
            }
        }
        
        void SavePicks()
        {
            foreach( DraftSelect myTurn in participants )
            {
                string fileName = $"{myTurn.Name}.txt";
                using( FileStream stream = new FileStream( fileName, FileMode.Create, FileAccess.Write ) )
                using( StreamWriter writer = new StreamWriter( stream ) )
                {   
                    int i = 0;
                    foreach( Player player in myTurn.Selections )
                    {
                        writer.WriteLine( $"Round{i+1}: {player.Name} {player.Team} ({player.Position})" );
                        i ++;
                    }
                    while( i < totalRounds )
                    {
                        writer.WriteLine( $"Round{i+1}: " );
                        i ++;
                    }
                }
            }
        }
        
        public void RunDraft(List<Player> playerList)
        {
            for(int n = 1; n <= TotalRounds; n ++)
            {
                if( n%2 == 1)   //It's an odd round, going from 1 to n
                {
                    for(int i = 0; i < participants.Length; i ++ )
                    {
                        SelectPlayer( playerList, participants[i] );
                        DisplaySelections( participants[i] );
                        currentPick ++;
                    }
                }
                
                if( n%2 == 0)   //Going from n to 1
                {
                    for(int i = participants.Length; i > 0; i -- ) 
                    {
                        SelectPlayer( playerList, participants[i-1] );
                        DisplaySelections( participants[i-1] );
                        currentPick ++;
                    }     
                }
                SavePicks();
                currentRound ++;
            }
        }
    }
    
    static class Program 
    {
        static Program( ) { OutputEncoding = System.Text.Encoding.Unicode; }
        static void Main( )
        {
			WelcomeMessage();
            DraftManager draftManager = GetPlayerSettings();
            List< Player > playerList = DraftManager.GenerateNewPlayerList("NhlPlayerList.csv");
            DraftManager.WritePlayerList(playerList);
            
            
            draftManager.RunDraft(playerList);
            
            
            //Drafter selects Player
            //Player added to drafter's list
            //Turn changes to next person
            //Must be able to search through players during drafting process
        }
        
        static void WelcomeMessage()
        {
            WriteLine("\nWelcome to the Ho Cup Draft 2017-18!\n");
        }
        static DraftManager GetPlayerSettings()
        {
            WriteLine("Gathering settings for the draft...");
            Write("\nPlease enter the number of people participating in the draft: ");
            int participNum = int.Parse(ReadLine());    //Defensive programming here
            Write("Please enter the number of rounds in this draft: ");
            int totalRounds = int.Parse(ReadLine());
            Write("Please enter the minimum number of defensemen a participant must draft: ");
            int numDef = int.Parse(ReadLine());
            
            Write("Entering participant names... Press 'return' to continue: ");
            ReadLine(); 
            Clear();
            
            DraftSelect[] participants = new DraftSelect[participNum];  //Create array of participants
            for(int i = 0; i < participants.Length; i ++ )
            {
                Write( $"Enter name for Participant {i+1}: " );
                string name = ReadLine();
                participants[i] = new DraftSelect( name, i+1 );
                WriteLine(participants[i]);
            }
            
            DraftManager draftManager = new DraftManager( participants, totalRounds, numDef );
            return draftManager;
        }
    }
}
