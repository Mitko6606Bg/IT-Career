#include <iostream>
#include <fstream>
#include <string>
#include <random>
#include <ctime>
#include <cstring> 
#include <filesystem> // for fs na dali sashtestvuva faila

using namespace std;

struct Abonat {
	string contractNumber;
	int contractLength = 0;
	string Name;
	string phoneNumber;
	int conversationsHeld = 0;
	int conversationsArray[100] = { 0 };
	double priceForMinute = 0.00;
	double monthlyFee = 0.00;
};


//da dobavq generate na convoto i array na convoto i na read from file
int generateRandomNumber(int lower, int upper, bool useSeed = true) {
    // Use current time as seed if useSeed is true
    static std::default_random_engine generator;
    if (useSeed) {
        generator.seed(static_cast<unsigned int>(std::time(0)));
    }

    // Create a uniform distribution between the lower and upper bounds
    std::uniform_int_distribution<int> distribution(lower, upper);

    // Generate and return a random number
    return distribution(generator);
}

void ClearCin() {
    cin.clear();
    cin.ignore(numeric_limits<streamsize>::max(), '\n');
}

void AddAbonat(Abonat abonati[], const int maxAbonati, int& allEnteredAbonats) {

    int contractNumberInput = 0;
    int contractLengthInput = 0;
    string phoneNumberInput;
    string nameInput;
    int priceForMinuteInput = 0;
    int monthlyFeeInput = 0;
    bool validContractN = false;
    bool validContractLength = false;
    bool validPhoneNumber = false;
    bool validPriceForMinute = false;
    bool validMonthlyFee = false;
    bool validName = false;
    int nAbonati = 0;
    int availableAbonats = 30 - allEnteredAbonats;
    int keepIndex = nAbonati + allEnteredAbonats;
   

    if (allEnteredAbonats < 30) {
        cout << "Kolko abonata shte dobavqte? - " << availableAbonats << " svobodni\n";


        do {
            cin >> nAbonati;

            if (nAbonati < 1 || nAbonati > availableAbonats) {
                cout << "Ne moje da se dobavqt poveche ot - " << availableAbonats << " abonata !!! \n";
            }

        } while (nAbonati < 1 || nAbonati > availableAbonats);


        for (int i = allEnteredAbonats; i <= keepIndex; i++) {
            cout << "\nEntering details for abonat " << (i + 1) << ":\n";

            cout << "Nomer na dogovor: ";
 
            string contractNumberInputAsString;
            do {

                cin >> contractNumberInput;

                if (cin) {

                    contractNumberInputAsString = to_string(contractNumberInput);

                    if (contractNumberInputAsString.length() == 6) {
                        abonati[i].contractNumber = "BG" + contractNumberInputAsString;
                        
                        validContractN = true;
                    }
                    else {
                        cout << "nomera na dogovora trqbva da e 6 chisla !!! \n";
                    }
                }
                else {
                    cout << "nomera trqbva da e chislo\n";
                    ClearCin();
                }

            } while (!validContractN);


            cout << "Srok ( 1 godina | 2 godini ): ";

            do {

                cin >> contractLengthInput;

                if (cin) {

                    if (contractLengthInput == 1 || contractLengthInput == 2) {
                        abonati[i].contractLength = contractLengthInput;
                        validContractLength = true;
                    }
                    else {
                        cout << "sroka moje da e samo 1 ili 2 godini !!!\n";
                    }

                }
                else {
                    cout << "sroka trqbva da e chislo\n";
                    ClearCin();
                }



            } while (!validContractLength);


            cout << "Ime: ";

            do
            {
                bool isNumber = false;

                cin >> nameInput;

                for (char c : nameInput) {
                    if (isdigit(c)) {
                        isNumber = true;
                    }
                }

                if (!isNumber) {
                    abonati[i].Name = nameInput;
                    validName = true;
                }
                else {
                    cout << "V imeto ne trqbva da ima chisla\n";
                    ClearCin();
                }

            } while (!validName);


            cout << "Phone number: ";

            do {

                cin >> phoneNumberInput;

                if (phoneNumberInput.length() == 10) {

                    if (phoneNumberInput.substr(0, 2) == "08") {
                        abonati[i].phoneNumber = phoneNumberInput;
                        validPhoneNumber = true;
                    }
                    else {
                        cout << "Nomera trqbva da zapochva s 08 !!!\n";
                    }
                }
                else {
                    if (phoneNumberInput.substr(0, 2) == "08") {
                        abonati[i].phoneNumber = phoneNumberInput;
                    }
                    else {
                        cout << "Nomera trqbva da zapochva s 08 !!!\n";
                    }
                    cout << "Nomera trqbva da e 10 chisla !!!\n";
                }
                

            } while (!validPhoneNumber);

            abonati[i].conversationsHeld = generateRandomNumber(1, 100);

            for (int j = 0; j < abonati[i].conversationsHeld; j++)
            {
                abonati[i].conversationsArray[j] = generateRandomNumber(30, 3600);
            }

            cout << "Taksa na minuta: ";



            do
            {
                cin >> priceForMinuteInput;

                if (cin)
                {
                    abonati[i].priceForMinute = priceForMinuteInput;
                    validPriceForMinute = true;
                }
                else {
                    cout << "Cenata trqbva da e chislo \n";
                    ClearCin();
                }

            } while (!validPriceForMinute);
            

            cout << "Mesechna taksa: ";

            do
            {
                cin >> monthlyFeeInput;

                if (cin)
                {
                    abonati[i].monthlyFee = monthlyFeeInput;
                    validMonthlyFee = true;
                }
                else {
                    cout << "Taksata trqbva da e chislo \n";
                    ClearCin();
                }

            } while (!validMonthlyFee);

          

            allEnteredAbonats++;
        }


        cout << "-------------------------------------- \n"
            << "\nYou have entered the following details:\n";

        for (int i = 0; i < nAbonati; i++) {
            cout << "\nAbonat " << (i + 1) << ":\n";
            cout << "Nomer na dogovor: " << abonati[i].contractNumber << endl;
            cout << "Srok: " << abonati[i].contractLength << endl;
            cout << "Ime: " << abonati[i].Name << endl;
            cout << "Phone number: " << abonati[i].phoneNumber << endl;
            cout << "Taksa na minuta: " << abonati[i].priceForMinute << endl;
            cout << "Mesechna taksa: " << abonati[i].monthlyFee << endl;
        }
        cout << "\n-------------------------------------- \n";
    }
    else {
        cout << "Nqma mqsto za novi abonati MAX(30)\n";
        return;
    }
    
}

void ListAllAbonati(Abonat abonati[], int allEnteredAbonats) {
    if (allEnteredAbonats == 0) {
        cout << "No abonati in list!\n";
        return;
    }

    cout << "\n-------------------------------------- \n";
    cout << "\nList of abonati:\n";
    for (int i = 0; i < allEnteredAbonats; i++) {
        cout << "\n---------< Abonat " << (i + 1) << " >-----------------\n"
            << "Nomer na dogovor: " << abonati[i].contractNumber << endl
            << "Srok: " << abonati[i].contractLength << endl
            << "Ime: " << abonati[i].Name << endl
            << "Phone number: " << abonati[i].phoneNumber << endl
            << "Provedeni razgovori: " << abonati[i].conversationsHeld << endl;
        printf("Taksa na minuta: %.2f lv.\n", abonati[i].priceForMinute);
        printf("Mesechna taksa: %.2f lv.\n", abonati[i].monthlyFee);

    }
    cout << "\n-------------------------------------- \n";
}

void ListAbonati(Abonat abonati[], int id) {
    cout << "\n---------< Abonat " << (id + 1) << " >-----------------\n"
        << "Nomer na dogovor: " << abonati[id].contractNumber << endl
        << "Srok: " << abonati[id].contractLength << endl
        << "Ime: " << abonati[id].Name << endl
        << "Phone number: " << abonati[id].phoneNumber << endl
        << "Provedeni razgovori: " << abonati[id].conversationsHeld << endl;
     printf("Taksa na minuta: %.2f lv.\n", abonati[id].priceForMinute);
     printf("Mesechna taksa: %.2f lv.\n", abonati[id].monthlyFee);
     cout << "\n--------------------------------------\n";
}

void SearchForPhoneNumber(Abonat abonati[], int allEnteredAbonats) {

    string validSearchPhoneNumber;
    string inputPhoneNumber;
    bool validPhoneNumber = false;

    cout << "Search by phone number: ";

    do {

        cin >> inputPhoneNumber;

        if (inputPhoneNumber.length() == 10) {

            if (inputPhoneNumber.substr(0, 2) == "08") {
                validSearchPhoneNumber = inputPhoneNumber;
                validPhoneNumber = true;
            }
            else {
                cout << "Nomera trqbva da zapochva s 08 !!!\n";
            }
        }
        else {
            if (inputPhoneNumber.substr(0, 2) == "08") {
                validSearchPhoneNumber = inputPhoneNumber;
            }
            else {
                cout << "Nomera trqbva da zapochva s 08 !!!\n";
            }
            cout << "Nomera trqbva da e 10 chisla !!!\n";
        }

    } while (validPhoneNumber == false);


    for (int i = 0; i < allEnteredAbonats; i++)
    {
        if (abonati[i].phoneNumber == validSearchPhoneNumber) {
            ListAbonati(abonati, i);
        }
        else {
            cout << "Abonat not found !!!\n";
        }
    }

}

void FindHighestPricePerMinute(Abonat abonati[], int allEnteredAbonats) {

    if (allEnteredAbonats == 0) {
        cout << "Nqma vavedeni abonati !!!\n";
        return;
    }

    double highestPricePerMinute = 0;
    int idOfHighestPrice = 0;

    for (int i = 0; i < allEnteredAbonats; i++)
    {
        if (abonati[i].priceForMinute > highestPricePerMinute) {
            highestPricePerMinute = abonati[i].priceForMinute;
            idOfHighestPrice = i;
        }

    }

    ListAbonati(abonati, idOfHighestPrice);
    

}

void SortByMontlyFee(Abonat abonati[], int allEnteredAbonats) {

    if (allEnteredAbonats == 0) {
        cout << "No entered abonati!!!\n";
        return;
    }

    for (int i = 0; i < allEnteredAbonats - 1; ++i) {
        for (int j = 0; j < allEnteredAbonats - i - 1; ++j) {
            if (abonati[j].monthlyFee > abonati[j + 1].monthlyFee) {
                Abonat temp = abonati[j];
                abonati[j] = abonati[j + 1];
                abonati[j + 1] = temp;
            }
        }
    }
    
    cout << "Abonati podredeni po taksa na mesec !!! \n";

}

void SortByName(Abonat abonati[], int allEnteredAbonats) {

    if (allEnteredAbonats == 0) {
        cout << "No entered abonati!!!\n";
        return;
    }

    if (!abonati == 0) {
        for (int i = 0; i < allEnteredAbonats - 1; ++i) {
            for (int j = 0; j < allEnteredAbonats - i - 1; ++j) {
                if (abonati[j].Name > abonati[j + 1].Name) {
                    Abonat temp = abonati[j];
                    abonati[j] = abonati[j + 1];
                    abonati[j + 1] = temp;
                }
            }
        }

        ListAllAbonati(abonati, allEnteredAbonats);
    }
    else {
        cout << "Nqma abonati s dva godishni dogovora !\n";
    }

}

//void WriteToFile(Abonat abonati[], int allEnteredAbonats) {
//    if (allEnteredAbonats == 0) {
//        cout << "No abonati to output!!!\n";
//        return;
//    }
//
//    ofstream MyFile("abonati.txt");
//
//    for (int i = 0; i < allEnteredAbonats; i++) {
//
//        MyFile << "\nAbonat " << (i + 1) << ":\n"
//               << "Nomer na dogovor: " << abonati[i].contractNumber << endl
//               << "Srok: " << abonati[i].contractLength << endl
//               << "Ime: " << abonati[i].Name << endl
//               << "Phone number: " << abonati[i].phoneNumber << endl
//               << "Taksa na minuta: " << abonati[i].priceForMinute << endl
//               << "Mesechna taksa: " << abonati[i].monthlyFee << endl
//               << "Provedeni razgovori: " << abonati[i].conversationsHeld << endl;
//    }
//
//    MyFile.close();
//}

void ConvertStringToCharArray(const string& str, char* charArray, int size) {
    strncpy_s(charArray, size, str.c_str(), size - 1);
    charArray[size - 1] = '\0'; 
}

void WriteToFile(Abonat abonati[], int allEnteredAbonats) {

    if (allEnteredAbonats == 0) {
        cout << "No abonati to output!!!\n";
        return;
    }

    ofstream MyFile("abonati.bin", ios::binary);

    if (!MyFile) {
        cout << "File not found.\n";
        return;
    }


    for (int i = 0; i < allEnteredAbonats; i++) {
        // Write contractNumber length and data
        size_t contractNumberLength = abonati[i].contractNumber.size();
        MyFile.write(reinterpret_cast<char*>(&contractNumberLength), sizeof(contractNumberLength));
        MyFile.write(abonati[i].contractNumber.c_str(), contractNumberLength);

        // Write other data
        MyFile.write(reinterpret_cast<char*>(&abonati[i].contractLength), sizeof(abonati[i].contractLength));

        // Write Name and phoneNumber lengths and data
        size_t nameLength = abonati[i].Name.size();
        size_t phoneLength = abonati[i].phoneNumber.size();
        MyFile.write(reinterpret_cast<char*>(&nameLength), sizeof(nameLength));
        MyFile.write(abonati[i].Name.c_str(), nameLength);
        MyFile.write(reinterpret_cast<char*>(&phoneLength), sizeof(phoneLength));
        MyFile.write(abonati[i].phoneNumber.c_str(), phoneLength);

        // Write numerical data
        MyFile.write(reinterpret_cast<char*>(&abonati[i].priceForMinute), sizeof(abonati[i].priceForMinute));
        MyFile.write(reinterpret_cast<char*>(&abonati[i].monthlyFee), sizeof(abonati[i].monthlyFee));
        MyFile.write(reinterpret_cast<char*>(&abonati[i].conversationsHeld), sizeof(abonati[i].conversationsHeld));
    }

    MyFile.close();
    cout << "Successfull write!\n";
}

//void ReadFromFile(Abonat abonati[], int& allEnteredAbonats) {
//    ifstream ReadFile("abonatiRead.txt");
//
//    if (!ReadFile) {
//        cout << "File not found.\n";
//        return;
//    }
//
//    int index = 0;
//
//    while (ReadFile >> abonati[index].contractNumber) {
//
//        ReadFile >> abonati[index].contractLength;
//
//        ReadFile.ignore();  
//        getline(ReadFile, abonati[index].Name);
//
//        getline(ReadFile, abonati[index].phoneNumber);
//
//        ReadFile >> abonati[index].priceForMinute;
//
//        ReadFile >> abonati[index].monthlyFee;
//
//        ReadFile >> abonati[index].conversationsHeld;
//
//        ReadFile.ignore();
//
//        index++;
//    }
//    allEnteredAbonats = index;
//    ReadFile.close();
//    cout << "Uspeshno dobaveni " << index << " abonata";
//}

void ConvertCharArrayToString(const char* charArray, string& str, int size) {
    str.assign(charArray, size);
}

void ReadFromFile(Abonat abonati[], int& allEnteredAbonats) {
    ifstream ReadFile("abonati.bin", ios::binary);

    if (!ReadFile) {
        cout << "File not found.\n";
        return;
    }

    int index = 0;

    while (ReadFile) {
        // Read contractNumber length and data
        size_t contractNumberLength;
        ReadFile.read(reinterpret_cast<char*>(&contractNumberLength), sizeof(contractNumberLength));
        if (ReadFile.eof()) break; // Stop reading if we reached the end of the file

        if (contractNumberLength > 100) { // Check if length exceeds buffer size
            cout << "Error: contractNumber length exceeds buffer size.\n";
            break;
        }
        char contractNumber[100];
        ReadFile.read(contractNumber, contractNumberLength);
        abonati[index].contractNumber = string(contractNumber, contractNumberLength);

        // Read other data
        ReadFile.read(reinterpret_cast<char*>(&abonati[index].contractLength), sizeof(abonati[index].contractLength));

        // Read Name and phoneNumber lengths and data
        size_t nameLength;
        ReadFile.read(reinterpret_cast<char*>(&nameLength), sizeof(nameLength));
        if (nameLength > 100) { // Check if length exceeds buffer size
            cout << "Error: Name length exceeds buffer size.\n";
            break;
        }
        char name[100];
        ReadFile.read(name, nameLength);
        abonati[index].Name = string(name, nameLength);

        size_t phoneLength;
        ReadFile.read(reinterpret_cast<char*>(&phoneLength), sizeof(phoneLength));
        if (phoneLength > 50) { // Check if length exceeds buffer size
            cout << "Error: phoneNumber length exceeds buffer size.\n";
            break;
        }
        char phone[50];
        ReadFile.read(phone, phoneLength);
        abonati[index].phoneNumber = string(phone, phoneLength);

        // Read numerical data
        ReadFile.read(reinterpret_cast<char*>(&abonati[index].priceForMinute), sizeof(abonati[index].priceForMinute));
        ReadFile.read(reinterpret_cast<char*>(&abonati[index].monthlyFee), sizeof(abonati[index].monthlyFee));
        ReadFile.read(reinterpret_cast<char*>(&abonati[index].conversationsHeld), sizeof(abonati[index].conversationsHeld));

        index++;
    }

    allEnteredAbonats = index;
    ReadFile.close();
    cout << "Successfully read " << index << " abonati.\n";
}

void FindContractsByYear(Abonat abonati[], int allEnteredAbonats , int year) {

    if (allEnteredAbonats == 0) {
        cout << "Nqma vavedeni abonati !!!\n";
        return;
    }

    Abonat abonatiTwoYear[30];
    int count = 0;
    bool validContractLength = false;
    int yearInput = 0;

    

    if (year == 1) {
        
        cout << "Tursi po srok ( 1 / 2 godini ): \n";

        do {

            cin >> yearInput;

            if (yearInput == 1 || yearInput == 2) {
                year = yearInput;
                validContractLength = true;
            }
            else {
                cout << "sroka moje da e samo 1 ili 2 godini !!!\n";
            }

        } while (validContractLength == false);
    }

    for (int i = 0; i < allEnteredAbonats; i++)
    {
        if (abonati[i].contractLength == year) {
            abonatiTwoYear[count] = abonati[i];
            count++;

        }
    }

    SortByName(abonatiTwoYear , count);

}

void SearchByMontlyFee(Abonat abonati[], int allEnteredAbonats) {

    if (allEnteredAbonats == 0) {
        cout << "Nqma vavedeni abonati !!!\n";
        return;
    }

    Abonat abonatiByMontlyFee[30];
    double montlyFee = 0.00;
    int count = 0;

    cout << "Tursi taksa nad - ";
    cin >> montlyFee;

    for (int i = 0; i < allEnteredAbonats; i++)
    {
        if (abonati[i].monthlyFee > montlyFee) {
            abonatiByMontlyFee[count] = abonati[i];
            count++;
        }
    }

    ListAllAbonati(abonatiByMontlyFee, count);
}

void PayMontlyBill(Abonat abonati[], int allEnteredAbonats) {

    string validSearchPhoneNumber;
    string inputPhoneNumber;
    bool validPhoneNumber = false;
    bool foundNumber = false;
    double bill = 0.00;
    

    cout << "Enter phone number: ";

    do {

        cin >> inputPhoneNumber;

        if (inputPhoneNumber.length() == 10) {

            if (inputPhoneNumber.substr(0, 2) == "08") {
                validSearchPhoneNumber = inputPhoneNumber;
                validPhoneNumber = true;
            }
            else {
                cout << "Nomera trqbva da zapochva s 08 !!!\n";
            }
        }
        else {
            if (inputPhoneNumber.substr(0, 2) == "08") {
                validSearchPhoneNumber = inputPhoneNumber;
            }
            else {
                cout << "Nomera trqbva da zapochva s 08 !!!\n";
            }
            cout << "Nomera trqbva da e 10 chisla !!!\n";
        }

    } while (validPhoneNumber == false);


    for (int i = 0; i < allEnteredAbonats; i++)
    {
        if (abonati[i].phoneNumber == validSearchPhoneNumber) {
            int minuti = 0;
            foundNumber = true;
            bill += abonati[i].monthlyFee;
            for (int j = 0; j < abonati[i].conversationsHeld; j++)
            {
                minuti += (abonati[i].conversationsArray[j] + 59) / 60;
            }
            
            bill += minuti * abonati[i].priceForMinute;

            printf("Trqbva da platite: %.2f lv.\n", bill);
            printf("Za %d razgovora na cena: %.2f obshto minuti izgovoreni: %d\n", abonati[i].conversationsHeld, abonati[i].priceForMinute, minuti);
            
            string choice;
            cout << "Shte zaplatite li sumata? y/n \n";
            do
            {
                cin >> choice;
            } while (choice != "y" && choice != "n");

            if (choice == "y") {

                for (int t = 0; t < abonati[i].conversationsHeld; t++) {
                    abonati[i].conversationsArray[t] = 0;
                }

                cout << "Uspeshno zaplatena smetka";
            }
            else {
                printf("Sroka za plashtane na sumata ot: %.2f lv. - e do 1 chislo na sledvashtiq mesec \n", bill);
            }

        }
        
    }
    if (foundNumber == false) {
        cout << "Abonat s takuv nomer ne e nameren !!!\n";
    }

}

void SecondMenu(Abonat abonati[], int allEnteredAbonats) {

    int choice = 0;

    do
    {

        cout << "\n--------------| Menu 2 |--------------\n";
        cout << "( 1 ) 2 godishni dogovori sortirani po ime\n";
        cout << "( 2 ) tursi po srok na dogovora\n";
        cout << "( 3 ) tursi po taksa na mesec \n";
        cout << "( 4 ) Plashtane na smetka \n";
        cout << "( 5 ) Back to Menu 1 \n";
        cout << "\n--------------------------------------\n";

        do
        {
            cin >> choice;

            if (choice < 1 || choice > 5) {
                cout << "Nqma takava opciq !!! \n";
            }

        } while (choice < 1 || choice > 5);

        switch (choice)
        {

        case 1:
            FindContractsByYear(abonati, allEnteredAbonats, 2);
            break;
        case 2:
            FindContractsByYear(abonati, allEnteredAbonats, 1);
            break;
        case 3:
            SearchByMontlyFee(abonati, allEnteredAbonats);
            break;
        case 4:
            PayMontlyBill(abonati , allEnteredAbonats);
            break;
        
        }

    } while (choice != 5);

}

int main()
{
    int choice = 0;
    int allEnteredAbonats = 0;
    const int maxAbonati = 30;
    Abonat abonati[maxAbonati];

    //ReadFromFile(abonati, allEnteredAbonats);

	do
	{

        cout << "\n---------------| Menu |---------------\n";
		cout << "( 1 ) Dobavqne na abonament \n";
		cout << "( 2 ) Izvejdane na vsichki abonamenti \n";
		cout << "( 3 ) Tursene po phone number \n";
		cout << "( 4 ) Nameri nai visoka cena za minuta\n";
		cout << "( 5 ) Sortirai po taksa na mesec\n";
		cout << "( 6 ) Zapishi abonati v .txt file \n";
		cout << "( 7 ) Dobavi abonati ot .txt file \n";
		cout << "( 8 ) Vtoro Menu 2 \n";
		cout << "( 9 ) Exit \n";
        cout << "\n--------------------------------------\n";
		
		do
		{
			cin >> choice;

			if (choice < 1 || choice > 9) {
				cout << "Nqma takava opciq !!! \n";
			}

		} while (choice < 1 || choice > 9);

		switch (choice)
		{
		    case 1:
		    	AddAbonat(abonati , maxAbonati , allEnteredAbonats);
		    	break;
            case 2:
                ListAllAbonati(abonati, allEnteredAbonats);
                break; 
            case 3:
                SearchForPhoneNumber(abonati, allEnteredAbonats);
                break;
            case 4:
                FindHighestPricePerMinute(abonati, allEnteredAbonats);
                break;
            case 5:
                SortByMontlyFee(abonati, allEnteredAbonats);
            case 6:
                WriteToFile(abonati, allEnteredAbonats);
            case 7:
                ReadFromFile(abonati, allEnteredAbonats);
                break;
            case 8:
                SecondMenu(abonati, allEnteredAbonats);
                break;
		}

	} while (choice != 9);


    //WriteToFile(abonati, allEnteredAbonats);
    return 0;
}

