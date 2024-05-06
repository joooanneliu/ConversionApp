using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutputController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(string text)
        {
            string priceString = text.Replace(" ", ""); // Remove spaces

            string[] parts = priceString.Split('.');
            int totalDollars = int.Parse(parts[0]);
            int cents = 0;
            if(parts.Length > 1) {
                cents = int.Parse(parts[1]);
            }

            string ans = "";

            if(totalDollars == 0 && cents == 0) {
                ans = "zero dollars";
                return $"{ans}";
            }

            string[] units = { "million ", "thousand ", "" };
            string[] ones = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"};
            string[] tenNums = {"ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"};
            string[] tens = {"", "", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety"};

            int num = 100000000;
            int place = 0;
            bool tenOne = false; // if tens digit = 1
            bool tensExist = false; // if tens digit > 0

            int dollar = totalDollars;
            while (num > 0) {
                int digit = dollar / num;
                // Console.WriteLine($"{dollar} {num} {digit} {place}");

                if(digit > 0) {
                    if(place % 3 == 1) {
                        tensExist = true;
                        if (digit == 1) {
                            tenOne = true;
                        } else {
                            ans += tens[digit];
                        }
                    } else {
                        if(tenOne) {
                            ans += tenNums[digit] + " ";
                            tenOne = false;
                        } else {
                            if (tensExist) {
                                ans += "-";
                                tensExist = false;
                            }
                            ans += ones[digit] + " ";
                            // for place = 2, 5, 8;
                            if(place % 3 == 2) {
                                ans += units[place / 3];
                            }
                            if (place % 3 == 0) {
                                ans += "hundred ";
                            }
                        }
                    }
                }
                dollar = dollar % num;
                num /= 10;
                place += 1;
            }

            if(totalDollars > 0) {
                if(totalDollars == 1) {
                    ans += "dollar";
                } else {
                    ans += "dollars";
                }
            }

            if(cents > 0) {
                if(totalDollars > 0) {
                    ans += " and ";
                }
                if(cents > 10) {
                    int digit = cents / 10;
                    if(digit == 1) {
                        ans += tenNums[cents % 10] + " cents";
                    } else {
                        ans += tens[digit] + "-" + ones[cents % 10] + " cents";
                        
                    }
                } else {
                    ans += ones[cents];
                    if(cents == 1) {
                        ans += " cent";
                    } else {
                        ans += " cents";
                    }
                }
            }
            // string output = ans.toString();
            return $"{ans}";
            // Console.WriteLine(ans);  
        }
    }
}
