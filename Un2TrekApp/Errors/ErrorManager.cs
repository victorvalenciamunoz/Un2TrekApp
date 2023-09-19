using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un2TrekApp.Errors;

public class ErrorManager
{
    public List<Un2TrekError> ErrorList { get; private set; }

    public ErrorManager()
    {
        SetErrorList();
    }
    
    public Un2TrekError GetError(string code)
    {
        var error = ErrorList.Where(c=> c.Code.Equals(code)).FirstOrDefault();
        if (error != null)
        {
            return error;
        }

        error = ErrorList.Where(c => c.Code.Equals("G999")).FirstOrDefault();

        return error;
    }
    
    private void SetErrorList()
    {
        
        ErrorList = new()
        {
            new Un2TrekError
            {
                Code = "G999",
                Message = "Error not defined"
            }            
        };
        SetUserErrorList();
        SetTrekiErrorList();
        SetActivityErrorList();
        SetValidationErrorList();
    }

    private void SetActivityErrorList()
    {
        if (ErrorList == null)
            ErrorList = new();

        ErrorList.Add(new Un2TrekError
        {
            Code = "A001",
            Message = "Actitity not found"
        });
        ErrorList.Add(new Un2TrekError
        {
            Code = "A002",
            Message = "Activity with no Trekis"
        });
    }

    private void SetTrekiErrorList()
    {
        if (ErrorList == null)
            ErrorList = new();

        ErrorList.Add(new Un2TrekError
        {
            Code = "T001",
            Message = "Treki not found"
        });
        ErrorList.Add(new Un2TrekError
        {
            Code = "T002",
            Message = "Invalid distance"
        });
        ErrorList.Add(new Un2TrekError
        {
            Code = "T003",
            Message = "Treki not found in activity"
        });
        ErrorList.Add(new Un2TrekError
        {
            Code = "T004",
            Message = "Treki already captured"
        });
    }

    private void SetUserErrorList()
    {
        if (ErrorList == null)
            ErrorList = new();

        ErrorList.Add(new Un2TrekError
        {
            Code = "U001",
            Message = "User not found"
        });
        ErrorList.Add(new Un2TrekError
        {
            Code = "U002",
            Message = "Incorrect credentials"
        });
    }

    private void SetValidationErrorList()
    {
        if (ErrorList == null)
            ErrorList = new();

        ErrorList.Add(new Un2TrekError
        {
            Code = "V001",
            Message = "Some required fields are missing"
        });
    }

}