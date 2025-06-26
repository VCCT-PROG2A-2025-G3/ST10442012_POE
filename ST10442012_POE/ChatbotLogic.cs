using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;


//--------------------------------|| Description ||---------------------------------

// ChatbotLogic is the main class responsible for driving the Cybersecurity Awareness Chatbot.
// It processes user input, detects intent using keyword-based NLP, and provides spoken and textual responses
// tailored to cybersecurity education.
//
// Key Features:
// - Handles conversation flow (name input, skill level, topic selection, Q&A).
// - Responds to cybersecurity topics like phishing, social engineering, safe browsing, and more.
// - Uses baSic Natural Language Processing techniques:
//   • Input sanitization and tokenization for text normalization.
//   • Synonym matching via a configurable dictionary for broader intent coverage.
//   • Typo tolerance using Levenshtein Distance for fuzzy keyword recognition.
//   • Clarification handling to provide varied follow-up answers.
// - Incorporates speech synthesis via System.Speech.Synthesis for accessibility.
// - Tracks user-specific context (name, skill level, interest) to personalize responses.
//
// This class enables an interactive and informative experience aimed at improving user cybersecurity awareness.







namespace ST10442012_POE
{
    public class ChatbotLogic
    {
        // ---|| Fields ||---

        // Stores the user's name
        private string userName = "";

        // Stores the user's skill level (beginner, intermediate, advanced)
        private string userSkillLevel = "";

        // Stores the user's favorite cybersecurity topic
        private string favoriteTopic = "";

        // <summary>
        // Sets the user's favorite topic if the input is not null or whitespace.
        // This helps personalize future chatbot responses.
        // </summary>
        public void SetFavoriteTopic(string topic)
        {
            if (!string.IsNullOrWhiteSpace(topic))
                favoriteTopic = topic;
        }

        // Index of the last response given, used to avoid repeating the same response consecutively
        private int lastResponseIndex = -1;

        // Stores the list of possible responses from the last matched keyword group
        private List<string> lastResponses = null;

        // Random number generator used for selecting varied responses
        private readonly Random rand = new Random();


        // <summary>
        // A dictionary mapping common user emotions (detected through keywords)
        // to personalized response templates that include the user's name.
     
        // NLP Feature: Sentiment-based response generation.
        // This enables the chatbot to provide empathetic and context-aware replies
        // when users express emotions like fear, confusion, or frustration.
        
        private readonly Dictionary<string, string> sentimentTemplates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "worried", "It's completely understandable to feel that way, {0}. Scammers can be very convincing. Let me share some tips to help you stay safe." },
            { "anxious", "Don't worry, {0}. Cybersecurity can seem overwhelming, but I'm here to guide you step by step." },
            { "frustrated", "I get that cybersecurity can be frustrating, {0}. Let's tackle your concerns together." },
            { "confused", "It's okay to feel confused, {0}. I'm here to explain things as simply as possible." },
            { "curious", "Curiosity is great, {0}! Ask me anything about cybersecurity and I'll do my best to help." },
            { "scared", "It's normal to feel scared about online threats, {0}. With the right knowledge, you can protect yourself." },
            { "overwhelmed", "Take a deep breath, {0}. We'll go through cybersecurity topics at your pace." },
            { "hopeful", "It's good to stay hopeful, {0}. Cybersecurity is something you can master over time." },
            { "angry", "I understand it can be frustrating, {0}. Let's work together to make things clearer." },
            { "doubtful", "It's okay to have doubts, {0}. Feel free to ask any questions you have." },
            { "tired", "I know this can be tiring, {0}. Let's take it one step at a time." },
    
        };

        // <summary>
        // A list of phrases that indicate the user is asking for further explanation or clarification.
        // NLP Feature: Clarification detection (Intent classification).
        // This allows the chatbot to detect follow-up questions and provide alternative or simplified responses
        // to help users understand complex topics.
       
        private readonly List<string> clarificationKeywords = new List<string>
    {
        "tell me more", "more details", "explain further", "i'm confused",
        "can you explain", "what do you mean", "clarify", "elaborate",
        "expand", "give me another example", "another example",
        "can you give more info", "can you elaborate", "can you clarify",
        "could you explain more", "help me understand", "i don't get it",
        "i do not get it", "not clear", "please explain", "please clarify",
        "can you say that differently", "say it differently",
        "can you break it down", "break it down", "can you simplify",
        "explain in simple terms", "say that again", "repeat please",
        "can you go slower", "dumb it down", "say it slower",
        "i'm lost", "i'm not sure", "make it simpler", "walk me through",
        "step by step", "what does that mean", "rephrase please"
    };

        //<summary>
        // A list of keyword-response pairs used to match user input to appropriate responses.
        // NLP Feature: Keyword-based intent recognition.

        // This allows the chatbot to recognize user intent based on specific keywords or phrases
        // and provide relevant responses for various cybersecurity topics.
        // This is the core of the chatbot's natural language processing capabilities.
       
        private readonly List<(string[] Keywords, List<string> Responses)> keywordResponses = new List<(string[], List<string>)>
        {
            // ----------|| General Cybersecurity Awareness ||----------
                #region GENERAL CYBERSECURITY AWARENESS QUESTIONS AND RESPONSES
                (
                    new[] { "importance of cybersecurity", "why do we need cybersecurity", "why is cybersecurity important" },
                    new List<string>
                    {
                        "Cybersecurity is essential to protect sensitive data like passwords, bank details, and personal information from hackers and fraudsters.",
                        "Without cybersecurity, your personal and financial information could be easily stolen by hackers.",
                        "Cybersecurity helps prevent unauthorized access to your digital life."
                    }
                ),
                (
                    new[] { "cyber threats", "common cyber attacks", "kinds of threats", "types of cyber threats" },
                    new List<string>
                    {
                        "Common threats include viruses, malware, phishing, ransomware, and DDoS attacks.",
                        "Phishing, malware, and ransomware are some of the most common cyber threats.",
                        "Cyber threats can harm your data or steal information, so always stay alert."
                    }
                ),
                (
                    new[] { "boost online security", "how to improve cybersecurity", "increase security" },
                    new List<string>
                    {
                        "Keep software updated, use strong passwords, enable 2FA, and stay cautious online.",
                        "Regularly update your devices and use unique passwords for each account.",
                        "Enable two-factor authentication and avoid clicking on suspicious links."
                    }
                ),
                (
                    new[] { "cybersecurity", "digital protection", "internet security", "online safety" },
                    new List<string>
                    {
                        "Cybersecurity is the practice of protecting systems, networks, and data from digital attacks.",
                        "It helps keep your personal information safe from hackers and cybercriminals.",
                        "Practicing good cybersecurity habits is essential in today's digital world."
                    }
                ),(
                    new[] { "what is a hacker", "who are hackers", "are hackers dangerous", "hacker meaning" },
                    new List<string>
                    {
                        "A hacker is someone who uses their technical skills to gain unauthorized access to systems or data.",
                        "Not all hackers are bad — some help improve security. But malicious hackers can steal data or cause damage."
                    }
                ),

                //---What is encryption / why is encryption important?---
                (
                    new[] { "what is encryption", "define encryption", "why is encryption important", "encryption meaning" },
                    new List<string>
                    {
                        "Encryption is the process of converting information into a code to prevent unauthorized access.",
                        "It helps protect sensitive data like messages, passwords, and financial information from being stolen."
                    }
                ),

                //---Tips for being safe online in general---
                (
                    new[] { "online safety tips", "cybersecurity tips", "how to stay safe online", "safety online", "internet safety tips" },
                    new List<string>
                    {
                        "Use strong passwords, avoid suspicious links, and don’t share personal info with untrusted sources.",
                        "Keep your software updated and use antivirus protection to stay safe online.",
                        "Don’t reuse passwords and enable two-factor authentication wherever possible."
                    }
                ),

                //---Cyberbullying / harassment online---
                (
                    new[] { "cyberbullying", "online bullying", "internet harassment", "how to deal with online bullying" },
                    new List<string>
                    {
                        "Cyberbullying is when someone uses the internet to threaten, harass, or embarrass others.",
                        "If you're experiencing cyberbullying, block the person, save evidence, and report it to the platform or authorities."
                    }
                ),

                //---What is a firewall?---
                (
                    new[] { "what is a firewall", "firewall meaning", "firewall in cybersecurity", "do i need a firewall" },
                    new List<string>
                    {
                        "A firewall is a security system that monitors and controls incoming and outgoing network traffic.",
                        "It acts as a barrier between your device and potentially harmful content on the internet."
                    }
                ),
#endregion

                //---Malware---
                #region MALWARE QUESTIONS AND RESPONSES
                (
            new[] { "what is malware", "define malware", "malware meaning", "explain malware" },
            new List<string>
            {
                "Malware is malicious software designed to harm, exploit, or otherwise compromise your device or data.",
                "Common types of malware include viruses, worms, trojans, spyware, and ransomware."
            }
        ),
        (
            new[] { "what is ransomware", "define ransomware", "ransomware meaning", "explain ransomware" },
            new List<string>
            {
                "Ransomware is a type of malware that locks your files or device and demands payment to unlock them.",
                "Never pay the ransom. Instead, disconnect from the internet and seek professional help."
            }
        ),
        (
            new[] { "how to protect against malware", "malware protection", "prevent malware" },
            new List<string>
            {
                "Keep your software updated, use antivirus software, and avoid downloading files from untrusted sources.",
                "Be cautious with email attachments and links, and regularly back up your important data."
            }
        ),
        (
            new[] { "how to remove malware", "malware removal", "infected with malware" },
            new List<string>
            {
                "Run a full scan with your antivirus software and follow its instructions to remove threats.",
                "If the infection is severe, consider seeking help from a cybersecurity professional."
            }
        ),
        (
            new[] { "how to prevent ransomware", "ransomware protection", "stop ransomware" },
            new List<string>
            {
                "Regularly back up your data, keep your system updated, and be wary of suspicious emails and links.",
                "Use reputable security software and never enable macros in documents from unknown sources."
            }
            ),
        (
            new[] { "what is a trojan", "define trojan", "trojan meaning", "explain trojan" },

            new List<string>
            {
                "A trojan is a type of malware that disguises itself as legitimate software to trick users into installing it.",
                "Once installed, it can steal data, create backdoors, or cause other harm."
            }
        ),
        (
            new[] { "malware" }, // This will match anything that contains "malware"
            new List<string>
            {
                " Malware is software designed to harm or exploit your system.",
                "If you think you’re dealing with malware, scan your device with security software and avoid clicking unknown links."
            }
        ),
#endregion

                // ----------|| Passwords ||----------

                #region PASSWORD QUESTIONS AND RESPONSES
                (
            new[] { "what is password security", "how do i create a strong password", "what makes a password secure", "what are the qualities of a strong password" ,"Whats password security"},
            new List<string>
            {
                "Password security means protecting your online accounts by using strong, hard-to-guess passwords.",
                "A secure password is typically at least 12 characters long, doesn't use personal details, and combines uppercase and lowercase letters, numbers, and symbols.",
                "It's best to use a different password for every account to prevent one breach from affecting others."
            }
        ),
        (
            new[] { "is it safe to write down passwords", "can i store passwords on paper", "should i write my passwords down" },
            new List<string>
            {
                "Writing passwords down can be risky if others gain access to that paper.",
                "If you must write them down, store the list in a very secure, private location.",
                "A better and safer option is to use a trusted password manager."
            }
        ),
        (
            new[] { "what should i do if my password is hacked", "my password was compromised", "steps after a hacked password" },
            new List<string>
            {
                "If you think your password has been compromised, change it immediately on the affected account.",
                "Check if you’ve used that same password on other sites and change it there too.",
                "Enable two-factor authentication (2FA) and monitor your accounts for suspicious activity."
            }
        ),
        (
            new[] { "is it okay to share my password", "can i share my password with a friend", "why is sharing passwords risky" },
            new List<string>
            {
                "No, you should never share your passwords.",
                "Once your password is shared, you lose control over your account's security.",
                "Instead, use account-sharing features when available, and change the password afterward if needed."
            }
        ),
        (
            new[] { "what are common password mistakes", "how can i keep my password safe", "tips for secure passwords", "password security advice" },
            new List<string>
            {
                "Common mistakes include using short passwords, personal info, or the same password for multiple sites.",
                "Use long, complex, and unique passwords for each account.",
                "Avoid using words found in the dictionary and use a password manager to keep track of everything."
            }
        ),
        (
            new[] { "what is a password manager", "should i use a password manager", "benefits of password manager" },
            new List<string>
            {
                "A password manager securely stores and manages your passwords.",
                "It can generate strong, unique passwords for each account.",
                "Using a password manager is highly recommended for better security."
            }
        ),
        (
            new[] { "how often should i change my password", "password expiry", "should i change my password regularly" },
            new List<string>
            {
                "It's a good practice to change your passwords regularly, especially for important accounts.",
                "If you suspect any suspicious activity, change your password immediately."
            }
        ),
        (
            new[] { "what is two factor authentication", "what is 2fa", "should i use 2fa", "two step verification" },
            new List<string>
            {
                "Two-factor authentication (2FA) adds an extra layer of security by requiring a second form of verification.",
                "Always enable 2FA where possible to protect your accounts."
            }
        ),
        (
            new[] { "forgot my password", "how to recover my password", "reset password" },
            new List<string>
            {
                "If you forget your password, use the 'Forgot Password' or 'Reset Password' option on the login page.",
                "Follow the instructions sent to your email or phone to reset your password securely."
            }
        ),
        (
            new[] { "what is a strong password", "what is strong password", "what makes a password strong", "strong password definition", "password strength", "explain strong password", "how to create a strong password" },
            new List<string>
            {
                "A strong password is one that is difficult for others to guess or crack.",
                "It usually has at least 12 characters and includes a mix of uppercase letters, lowercase letters, numbers, and special symbols.",
                "Avoid using common words or personal info like names or birthdays."
            }
        ),

        (
            new[] { "password" }, // Matches anything that includes the word "password"
                        new List<string>
                {
                    " Strong passwords are key to protecting your online accounts.",
                    "If you need help with passwords, remember to use long, complex ones and avoid sharing them with anyone."
                }
            ),
            #endregion

                // -------------|| Phishing ||----------

                #region PHISHING QUESTIONS AND  RESPONSES


                (
            new[] { "what is phishing", "define phishing", "phishing meaning", "explain phishing","what is phishing"},
            new List<string>
            {
                "Phishing is a type of cyber attack where scammers try to trick you into giving personal info by pretending to be someone you trust.",
                "Phishing attacks often use fake emails or websites to steal your information."
            }
        ),

        (
            new[] { "phishing tip", "phishing tips", "phishing advice" },
            new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Always check the sender's email address carefully—phishers often use addresses that look similar to real ones.",
                "Never click on suspicious links or download attachments from unknown sources.",
                "Look for poor grammar or urgent requests in emails—these are common signs of phishing."
            }
        ),
        (
            new[] { "phishing","what is phishing", "define phishing", "phishing meaning", "explain phishing" },
            new List<string>
            {
                "Phishing is a type of cyber attack where scammers try to trick you into giving personal info by pretending to be someone you trust.",
                "Phishing attacks often use fake emails or websites to steal your information."
            }
        ),


        (
             new[] { "how to protect myself from phishing", "how to avoid phishing", "prevent phishing attacks" },
                    new List<string>
             {
                    "To protect yourself, always verify the sender's email address and avoid clicking on suspicious links.",
                    "Use security software and keep your devices updated to help detect and block phishing attempts."
             }
             ),


        (
            new[] { "detect phishing", "how do I know if it's phishing", "how to spot phishing", "phishing signs" },
            new List<string>
            {
                "Watch for suspicious emails with poor grammar, fake links, urgent messages, or unfamiliar senders.",
                "Always verify before clicking anything in an email."
            }
        ),
        (
            new[] { "clicked a fake link", "phishing mistake", "what happens if I click a phishing link" },
            new List<string>
            {
                "If you clicked a phishing link, disconnect from the internet, run a virus scan, and change your passwords immediately.",
                "Monitor your accounts for suspicious activity and consider contacting your bank if you entered financial information."
            }
        ),
        (
            new[] { "give example of phishing", "phishing email sample", "real phishing email" },
            new List<string>
            {
                "A common example: an email pretending to be from your bank asking you to confirm your password or click a suspicious link.",
                "Phishing emails often create a sense of urgency to trick you into acting quickly."
            }
        ),
        (
            new[] { "fake email", "phishing", "phishing attack", "phishing email", "scam" },
            new List<string>
            {
                "Phishing is when attackers pretend to be trustworthy sources to steal sensitive information, like passwords or personal data.",
                "Always be skeptical of unexpected emails asking for personal information."
            }
        ),


        (
            new[] { "what to do if i got phished","i gave my info to a scammer", "i was phished", "i shared my password", "help i got scammed" },
            new List<string>
            {
                "If you’ve shared sensitive info, change your passwords immediately and contact your bank if needed.",
                "Run a full security scan on your device and enable two-factor authentication where possible.",
                "Report the incident to your local cybercrime unit or service provider."
            }
        ),
        (
            new[] { "explain phishing to a child", "phishing for beginners", "simple phishing explanation" },
            new List<string>
            {
                "Phishing is like someone pretending to be your friend to trick you into giving your secrets.",
                "It’s when someone lies to you online to steal your money or passwords. Always be careful online!"
            }
        ),
        //---Catch-all for general phishing-related input---
                (
                new[] { "phishing" }, // Matches anything that includes the word "phishing"
                new List<string>
                {
                    "Phishing is when someone tries to trick you into sharing personal information, like passwords or bank details.",
                    "If you ever get a suspicious email or message, don’t click on any links and never share your login information."
                }
                )
            ,

                #endregion

                // ----------|| Browsing & Safe Internet Use ||----------
               # region SAFE BROWSING RESPONSES
                (
            new[] { "why is browser security important", "importance of secure browsing", "secure browsing benefits" },
            new List<string>
            {
                "Browser security helps protect your personal information, prevent tracking, and block malicious content.",
                "Without secure browsing, you’re more vulnerable to scams, viruses, and identity theft."
            }
        ),
        (
            new[] { "how to make browser more secure", "browser security settings", "secure my browser", "increase browser safety" },
            new List<string>
            {
                "You can increase browser safety by enabling pop-up blockers, disabling third-party cookies, and turning on safe browsing settings.",
                "Use extensions like ad blockers or privacy tools, and always keep your browser up to date."
            }
        ),


        (
            new[] { "how to browse safely", "internet safety tips", "safe browsing", "how to browse the internet safely", "browsing securely", "stay safe online" },
            new List<string>
            {
                "To browse safely, always use secure websites (look for HTTPS and a padlock).",
                "Avoid clicking suspicious links, keep your browser and software updated, and run antivirus protection.",
                "Be careful when using public Wi-Fi and consider using a VPN."
            }
        ),
        (
            new[] { "how to check if a website is secure", "safe websites", "what is https", "is this website safe" },
            new List<string>
            {
                "Check for HTTPS and a padlock icon in the address bar—that means your connection is encrypted.",
                "Avoid entering sensitive information on sites that are not secure."
            }
        ),
        (
            new[] { "what is phishing website", "spot fake websites", "phishing attack" },
            new List<string>
            {
                "Phishing websites try to steal your info by pretending to be real.",
                "Watch out for bad grammar, weird URLs, or login pages that don’t look right."
            }
        ),
        (
            new[] { "is public wifi safe", "public wifi", "public wi-fi", "wifi safety", "using internet in public", "safe on public wifi", "danger of public wifi" },
            new List<string>
            {
                "Public Wi-Fi can be risky because others might snoop on your data.",
                "Avoid logging into sensitive accounts or use a VPN for extra security when on public networks."
            }
        ),
        (
            new[] { "what is incognito mode", "is incognito safe", "does private mode protect me", "browsing in private mode", "incognito mode" },
            new List<string>
            {
                "Incognito mode hides your browsing history on your device but doesn’t stop websites, ISPs, or hackers from seeing what you do online.",
                "It's good for privacy on your device but not full protection."
            }
        ),
        (
            new[] { "what is a vpn", "is vpn safe", "do i need a vpn", "should i use a vpn", "vpn security" },
            new List<string>
            {
                "A VPN encrypts your internet traffic and hides your IP address, giving you more privacy.",
                "It's especially useful on public Wi-Fi or to access region-locked content."
            }
        ),
        (
            new[] { "can websites track me", "do websites track me", "how to stop tracking", "online tracking", "stop websites from tracking me" },
            new List<string>
            {
                "Yes, websites track you using cookies and scripts.",
                "To reduce tracking, use private browsing, block third-party cookies, or try privacy-focused browsers and extensions."
            }
        ),
        (
            new[] { "is it safe to save passwords in browser", "saving passwords", "browser saved passwords", "password safety online" },
            new List<string>
            {
                "Saving passwords in your browser is convenient but less secure.",
                "Using a dedicated password manager is safer and better for your security."
            }
        ),
        (
            new[] { "are ads safe", "popups safe", "should i click on ads", "clicking ads online" },
            new List<string>
            {
                "Be careful with ads and popups—they can sometimes lead to malware or scams.",
                "Consider using an ad blocker to reduce risks."
            }
        ),
        (
            new[] { "do i need antivirus", "antivirus for browsing", "is antivirus needed", "protect computer from virus" },
            new List<string>
            {
                "Yes! Antivirus software helps protect your device by blocking harmful websites and files while you browse.",
                "Keep your antivirus software up to date for the best protection."
            }
        ),

        (
        new[] {"safe browsing ", "what is safe browsing", "how to know if I'm browsing safely", "am I browsing safely", "is my browsing safe", "safe internet habits", "safe browsing meaning" },
            new List<string>
            {
                "Safe browsing means using the internet in a way that protects your personal data and avoids threats like scams or malware.",
                "You're browsing safely if you're using secure (HTTPS) websites, avoiding suspicious links, keeping your browser updated, and using antivirus protection.",
                "Good habits include not clicking on popups, using strong passwords, and avoiding risky websites or downloads."
            }
           ),
        (
            new[] { "browsing", "internet safety", "stay safe", "online safety" },
            new List<string>
            {
                "To stay safe online, always be cautious of what you click, use strong passwords, and avoid sharing personal info on unknown sites.",
                "Safe browsing includes using secure (HTTPS) websites, running antivirus protection, and avoiding public Wi-Fi without a VPN."
            }
        ),
                #endregion
              // ----------|| Common Basic Questions ||----------

               #region COMMON BASIC QUESTIONS AND RESPONSES
(
    new[] { "how are you", "how are you doing", "whats up", "wagwaan" },
    new List<string>
    {
        "I'm doing great, thank you for asking! How can I assist you today?",
        "I'm here and ready to help you with your cybersecurity questions.",
        "All systems are running smoothly! What would you like to know?"
    }
),
(
    new[] { "how can you help me", "how does this work", "what can you do", "what do you do", "what questions can i ask you" },
    new List<string>
    {
        "I can help you understand cybersecurity, avoid scams, create strong passwords, and stay safe online.",
        "Ask me anything about online safety, passwords, phishing, or privacy.",
        "I'm here to answer your cybersecurity questions and provide helpful tips."
    }
),
(
    new[] { "be secure online", "how do I stay safe online", "internet safety", "stay safe online" },
    new List<string>
    {
        "Use strong passwords, enable 2FA, avoid phishing links, don’t overshare online, and keep your devices updated.",
        "Always be cautious with emails and links, and keep your software up to date."
    }
),
(
    new[] { "signs of being hacked", "was I hacked", "how do I know if I was hacked" },
    new List<string>
    {
        "Signs include unexpected logins, password reset emails you didn’t request, unknown apps, or slow device performance.",
        "If you notice suspicious activity, change your passwords immediately and run a security scan."
    }
),
(
    new[] { "are you human", "are you a real person", "who are you" },
    new List<string>
    {
        "I’m CyboSecureBot — a smart digital assistant built to help you understand and protect yourself in the digital world.",
        "I'm an AI chatbot designed to help you with cybersecurity questions."
    }
),
(
    new[] { "are you real", "trust you", "is this legit", "are you safe" },
    new List<string>
    {
        "Yes! I'm designed to provide trusted cybersecurity advice. But always double-check online info—being cautious is smart!",
        "You can trust my advice, but always use your best judgment online."
    }
),
#endregion // COMMON BASIC QUESTIONS


                


            // ----------|| Social Engineering ||----------

                 #region SOCIAL ENGINEERING QUESTIONS AND RESPONSES
        (
            new[] { "what is social engineering", "define social engineering", "social engineering meaning", "explain social engineering" },
            new List<string>
            {
                "Social engineering is a technique attackers use to trick people into giving away confidential information.",
                "It's all about manipulating human psychology instead of hacking technology.",
                "Common examples include phishing, pretexting, and baiting."
            }
        ),
        (
            new[] { "how to recognize social engineering", "social engineering signs", "detect social engineering attack" },
            new List<string>
            {
                "Be cautious if someone pressures you for sensitive info or creates a false sense of urgency.",
                "Verify identities before sharing any personal or company data.",
                "Watch out for unusual requests or unexpected communications."
            }
        ),
        (
            new[] { "examples of social engineering", "social engineering types", "types of social engineering attacks" },
            new List<string>
            {
                "Examples include phishing emails, phone scams pretending to be IT support, and fake websites.",
                "Other types are baiting (offering freebies to get info) and tailgating (following someone into restricted areas)."
            }
        ),
        (
            new[] { "how to protect against social engineering", "prevent social engineering", "social engineering protection tips" },
            new List<string>
            {
                "Always verify the identity of anyone asking for sensitive info.",
                "Never share passwords or private data over phone or email unless you initiated the contact.",
                "Be skeptical of urgent or unusual requests and report suspicious activity."
            }
        ),
        #endregion // SOCIAL ENGINEERING

    
            // ----------|| Data Privacy & Protection ||----------

                #region DATA PRIVACY AND PROTECTION QUESTIONS AND RESPONSES
        (
            new[] { "what is data privacy", "define data privacy", "data privacy meaning" },
            new List<string>
            {
                "Data privacy means protecting personal information from unauthorized access or use.",
                "It ensures your personal data stays confidential and is handled responsibly."
            }
        ),
        (
            new[] { "how to protect personal data", "protect my data", "data protection tips" },
            new List<string>
            {
                "Use strong passwords and enable two-factor authentication wherever possible.",
                "Be careful what personal info you share online and regularly review privacy settings on social media.",
                "Keep your software up to date and use security tools to protect your devices."
            }
        ),
        (
            new[] { "what are privacy laws", "privacy regulations", "data protection laws" },
            new List<string>
            {
                "Privacy laws like GDPR (Europe) and POPIA (South Africa) regulate how personal data must be collected, stored, and used.",
                "These laws help protect individuals' rights and increase transparency from organizations."
            }
        ),
        (
            new[] { "how to know if my data is safe", "is my data protected", "check data privacy" },
            new List<string>
            {
                "Look for websites that use HTTPS and have clear privacy policies.",
                "Avoid sharing sensitive data on suspicious or untrusted websites.",
                "Use privacy-focused browsers and tools to limit data tracking."
            }
        ),
        #endregion 

        

            // ----------|| Mobile Security ||----------

                #region MOBILE SECURITY QUESTIONS AND RESPONSES
            (
                new[] { "how to keep my smartphone safe", "mobile security tips", "secure mobile phone" },
                new List<string>
                {
                    "Keep your phone’s software up to date to fix security vulnerabilities.",
                    "Only download apps from official app stores like Google Play or Apple App Store.",
                    "Use screen locks, strong passwords, or biometric security like fingerprint or face recognition.",
                    "Regularly back up your data to prevent loss in case of device issues."
                }
            ),
            (
                new[] { "mobile malware", "what is mobile malware", "mobile virus" },
                new List<string>
                {
                    "Mobile malware is malicious software designed to harm your smartphone or steal data.",
                    "It can come from suspicious apps, links, or emails. Always be cautious about what you download or click."
                }
            ),
            (
                new[] { "how to avoid mobile scams", "mobile phishing", "mobile scam prevention" },
                new List<string>
                {
                    "Don’t click links in suspicious texts or emails on your phone.",
                    "Verify caller identity before sharing personal info over phone.",
                    "Use security apps to scan for threats and avoid public Wi-Fi without VPN."
                }
            ),
            (
                new[] { "is public wifi safe on mobile", "using wifi on phone safe", "public wifi risks on mobile" },
                new List<string>
                {
                    "Public Wi-Fi can be risky for your phone because attackers might intercept your data.",
                    "Use a VPN and avoid logging into sensitive accounts when on public Wi-Fi.",
                    "Disable automatic connections to unknown Wi-Fi networks for extra safety."
                }
            ),
            #endregion // MOBILE SECURITY



            // ----------|| Miscellaneous ||---------

                #region Miscellaneous QUESTIONS AND RESPONSES 
                        (
                    new[] { "your name", "who are you", "who is this" },
                    new List<string>
                    {
                        "I am CyboSecureBot — your friendly cybersecurity assistant, created to help keep you safe online."
                    }
                ),
                    (
                    new[] { "time", "what time is it" },
                    new List<string>
                    {
                        $"The current time is {DateTime.Now:hh:mm tt}"
                    }
                ),
                (
                    new[] { "day", "today", "what day is it" },
                    new List<string>
                    {
                        $"Today is {DateTime.Now:dddd}"
                    }
                ),
                (
                    new[] { "month", "current month", "which month is it" },
                    new List<string>
                    {
                        $"It's {DateTime.Now:MMMM}"
                    }
                ),
                
                (
                    new[] { "ask you ","help", "what can I ask", "what questions can i ask you about", "what should I ask", "give me ideas on what i must ask you about" },
                    new List<string>
                    {
                        "You can ask me about cybersecurity, safe browsing, passwords, phishing, and much more!",
                        "Feel free to ask about online safety, password tips, or how to spot scams."
                    }
                ),
                (
                    new[] { "who created you", "who made you", "who developed you" },
                    new List<string>
                    {
                        "I was created by Kaveer Lala, a second-year student at Varsity College in Cape Town."
                    }
                ),
        #endregion

            
    };

        private readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();



        // <summary>
        // Constructor initializes the SpeechSynthesizer and sets a neutral voice.
        // </summary>
        public ChatbotLogic()
        {
            synthesizer.SelectVoiceByHints(VoiceGender.Neutral);
        }

        // ---|| Public Methods for WPF Interaction ||---

        // <summary>
        // Returns the chatbot's initial greeting message prompting for the user's name.
        // </summary>
        // <returns>Welcome message string</returns>

        public string GetInitialGreeting()
        {
            return "Hello! I am the Cybersecurity Awareness Chatbot. To exit at any time, simply type 'exit'.\n\nLet's begin! What is your name?";
        }



        // <summary>
        //  Validates the user's input as a proper name.
        //  Ensures input is not null, empty, whitespace, and contains no digits.
        //</summary>
       
        public bool IsValidName(string input)   
        {
            return !string.IsNullOrWhiteSpace(input) && !input.Any(char.IsDigit);
        }


        // <summary>
        // Sets the user's name if valid and returns a personalized welcome message.
        // Also uses text-to-speech to speak the welcome message.
        // </summary>

        public string SetUserName(string input)
        {
            if (!IsValidName(input))
                return "Please enter a valid name using letters only.";

            userName = input;
            string message = $"Welcome, {userName}! It's a pleasure to meet you.";
            Speak(message);
            return message;
        }

        // <summary>
        // Validates the user's input for their skill level in cybersecurity.
        // Acceptable inputs are "Beginner", "Intermediate", or "Advanced" (case-insensitive).
        // If valid, sets the internal skill level and outputs the formatted skill level.
        // </summary>
        // <param name="input">User input string representing skill level</param>
        // <param name="skillLevel">Outputs the validated and formatted skill level</param>
        // <returns>Confirmation message or error message</returns>
        public string ValidateSkillLevel(string input, out string skillLevel)
        {
            skillLevel = "";
            // Check for empty or whitespace input
            if (string.IsNullOrWhiteSpace(input))
                return "Skill level cannot be empty.";

            // Check if user wants to exit
            if (input.Trim().ToLower() == "exit")
                return $"Goodbye {userName}! Stay safe online.";

            // Normalize input to lowercase for comparison
            // Validate skill level against accepted values

            string skill = input.ToLower();
            if (skill == "beginner" || skill == "intermediate" || skill == "advanced")
            {
                userSkillLevel = char.ToUpper(skill[0]) + skill.Substring(1);
                skillLevel = userSkillLevel;
                return $"Great! I'll keep in mind that you're at a {userSkillLevel} level.";
            }

            // Return error if input is invalid
            return "Please enter either Beginner, Intermediate, or Advanced.";
        }


        // <summary>
        // Validates the user's input for their favorite cybersecurity topic.
        // Checks against a predefined list of allowed topics.
        // If valid, sets the topic output parameter and returns a friendly confirmation message.
        // </summary>
        // <param name="input">User input string representing their favorite topic</param>
        // <param name="topic">Outputs the validated topic</param>
        // <param name="userName">The user's name, used for personalized messages</param>
        // <returns>Confirmation message if valid, or error message if invalid</returns>


        public string ValidateFavoriteTopic(string input, out string topic, string userName)
        {
            topic = "";
            var allowedTopics = new[]
            {
                 // List of allowed topics for user selection
                "passwords",
                "phishing",
                "malware",
                "safe browsing",
                "social engineering",
                "data privacy",
                "mobile security"
    };

            // Check for empty or whitespace input
            if (string.IsNullOrWhiteSpace(input))
                return "Topic cannot be empty.";

            // Handle user request to exit chat
            if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                return "Type 'exit' again if you'd like to leave the chat.";

            // Loop through allowed topics to check for match (case-insensitive)
            foreach (var t in allowedTopics)
            {
                if (input.Equals(t, StringComparison.OrdinalIgnoreCase))
                {
                    // Set the validated topic to output parameter
                    topic = t;

                    // Use topic variable here, not favoriteTopic (which is undefined)
                    string[] topicResponses = {
                $"Awesome! I'll remember that you're interested in {topic}. You can ask any questions on cybersecurity now.",
                $"Great! {topic} is such an important topic — You can ask any questions on cybersecurity now.",
                $"Cool! Let's dive deeper into {topic} whenever you're ready. You can ask any questions on cybersecurity now.",
                $"Perfect choice, {userName}! I'm ready to talk about {topic} anytime. You can ask any questions on cybersecurity now."
            };

                    // Select a random response from the array
                    var random = new Random();
                    return topicResponses[random.Next(topicResponses.Length)];
                }
            }

            // Return error if input topic does not match any allowed topics

            return "Please enter one of the suggested topics: Passwords, Phishing, Malware, Safe Browsing, Social Engineering, Data Privacy, or Mobile Security.";
        }




        // <summary>
        // Processes the user's raw input, applies NLP logic, and returns an appropriate chatbot response.
        // Handles exit commands, sentiment detection, clarification requests, keyword matching, and fallback replies.
        // </summary>
        // <param name="rawInput">The raw user input string</param>
        //
        //<returns>A chatbot response string based on the input</returns>
        public string HandleUserInput(string rawInput)
        {
            try
            {
                string userInput = SanitizeInput(rawInput);

                if (string.IsNullOrEmpty(userInput))
                    return "Please ask a question related to cybersecurity or type 'exit'.";

                if (userInput.Contains("exit"))
                {
                    string nextSkill = GetNextSkillLevel(userSkillLevel);
                    return $"Goodbye {userName}! I hope I helped increase your skill level from {userSkillLevel} to {nextSkill}. Stay safe online.";
                }

                // Sentiment Detection
                foreach (var kvp in sentimentTemplates)
                {
                    if (userInput.Contains(kvp.Key))
                        return string.Format(kvp.Value, userName);
                }

                // Clarification Request
                bool isClarification = clarificationKeywords.Any(kw => userInput.Contains(kw));
                if (isClarification && lastResponses != null && lastResponses.Count > 0)
                {
                    int newIndex;
                    do
                    {
                        newIndex = rand.Next(lastResponses.Count);
                    } while (newIndex == lastResponseIndex && lastResponses.Count > 1);

                    lastResponseIndex = newIndex;
                    string response = lastResponses[newIndex];
                    Speak(response);
                    return response;
                }







                // Keyword Matching
                foreach (var entry in keywordResponses)
                {
                    foreach (var keyword in entry.Keywords)
                    {
                        if (ContainsKeyword(userInput, keyword))

                        {
                            int newIndex;
                            do
                            {
                                newIndex = rand.Next(entry.Responses.Count);
                            } while (newIndex == lastResponseIndex && entry.Responses.Count > 1);

                            lastResponseIndex = newIndex;
                            lastResponses = entry.Responses;
                            string response = entry.Responses[newIndex];

                            bool prependIntro = false;
                            if (!string.IsNullOrEmpty(favoriteTopic))
                            {
                                prependIntro = userInput.Contains(favoriteTopic.ToLower());
                            }

                            if (prependIntro)
                                return $"As someone interested in {favoriteTopic}, {userName}, {response}";
                            else
                                return response;
                        }
                    }
                }

                return $"I’m not sure how to answer that one, {userName}. Please rephrase your question or check your spelling.\nMaybe try asking about passwords, phishing, malware or safe browsing — those are my specialties!";
            }
            catch (Exception ex)
            {
                return $"Whoops, {userName}! {ex.Message}";
            }
        }

        // ---|| Helper Methods ||---
        // <summary>
        // Sanitizes input by removing punctuation and trimming whitespace, returning lowercase string.
        // </summary>

        private string SanitizeInput(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString().ToLower().Trim();
        }


        // <summary>
        // Returns the next skill level in the progression path.
        // </summary> 

        private string GetNextSkillLevel(string currentLevel)
        {
            switch (currentLevel.ToLower())
            {
                case "beginner": return "Intermediate";
                case "intermediate": return "Advanced";
                case "advanced": return "Expert";
                default: return currentLevel;
            }
        }

        // <summary>
        // Uses the SpeechSynthesizer to asynchronously speak the given message.
        // </summary>
        private void Speak(string message)
        {
            Task.Run(() => synthesizer.SpeakAsync(message));
        }



        // <summary>
        // Splits input string into a list of tokens (words) for processing.
        // </summary>
        private List<string> Tokenize(string input)
        {
            return input.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // <summary>
        // Checks if the sanitized input contains the sanitized keyword, considering synonyms and minor typos (via Levenshtein distance).
        // </summary>
        private bool ContainsKeyword(string input, string keyword)
        {
            input = SanitizeInput(input);
            keyword = SanitizeInput(keyword);

            var inputTokens = Tokenize(input);
            var keywordTokens = Tokenize(keyword);

            if (input.Equals(keyword, StringComparison.OrdinalIgnoreCase) ||
           input.Contains(" " + keyword + " ") ||
           input.StartsWith(keyword + " ") ||
           input.EndsWith(" " + keyword))
                return true;


            //  Synonym match (also exact tokens only)
            if (synonymDictionary.TryGetValue(keyword, out var synonyms))
            {
                foreach (var synonym in synonyms)
                {
                    var synTokens = Tokenize(SanitizeInput(synonym));
                    if (synTokens.All(st => inputTokens.Contains(st)))
                        return true;
                }
            }

            // Optional: typo tolerance (Levenshtein distance)
            int maxDistance = 2;
            foreach (var token in inputTokens)
            {
                if (LevenshteinDistance(token, keyword) <= maxDistance)
                    return true;

                if (synonymDictionary.TryGetValue(keyword, out var syns))
                {
                    foreach (var syn in syns)
                    {
                        if (LevenshteinDistance(token, SanitizeInput(syn)) <= maxDistance)
                            return true;
                    }
                }
            }

            return false;
        }


        // Dictionary mapping main keywords to their synonyms to improve keyword matching
        // Keys represent primary cybersecurity topics, values are lists of related alternative terms
        // Used to recognize different ways users may refer to the same concept
        // Case-insensitive to allow flexible matching
        private readonly Dictionary<string, List<string>> synonymDictionary = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "cyber threats", new List<string> { "cyber attacks", "hacking attempts", "online threats", "security threats", "cybersecurity threats" } },
                { "passwords", new List<string> { "passcodes", "login credentials", "password", "account passwords", "user passwords" } },
                { "phishing", new List<string> { "scam emails", "fraudulent emails", "fake websites", "email scams", "phishing scams" } },
                { "malware", new List<string> { "viruses", "spyware", "ransomware", "trojans", "malicious software" } },
                { "safe browsing", new List<string> { "secure browsing", "internet safety", "online safety", "browsing securely", "safe internet use" } },
                { "firewall", new List<string> { "network firewall", "internet firewall", "security firewall", "firewall protection" } },
                { "encryption", new List<string> { "data encryption", "encrypting data", "encrypted communication", "secure encryption" } },
                { "two-factor authentication", new List<string> { "2FA", "multi-factor authentication", "MFA", "two-step verification" } },
                { "cyberbullying", new List<string> { "online bullying", "internet harassment", "digital bullying", "online harassment" } },
                { "antivirus", new List<string> { "anti-malware", "virus protection", "security software", "malware protection" } }
            };

        // <summary>
        // Computes the Levenshtein distance between two strings for typo-tolerant matching.
        // </summary>

        private int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }







    }
}
//---------- || End of ChatbotLogic.cs ||----------