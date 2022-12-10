using LanguageLearnBot_.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entity_Framework_Test;

public class UserLanguageLearner
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int WasSent { get; private set; }
    
    public bool TheEnd { get; private set; }


    public void NextWord()
    {
        
        if (this.WasSent == Program.WordCount)
        {

            TheEnd= true;
        }
        else
        {

            this.WasSent++;
        }
    }


    public UserLanguageLearner()
    {
        this.WasSent = 2;
    }

}

