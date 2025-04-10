using Api.GRRInnovations.SmartValidation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.GRRInnovations.SmartValidation.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Removed the invalid usage of ActionArguments as it is not available in ActionExecutedContext.
            // ActionArguments is available in ActionExecutingContext, so the validation logic should be moved to OnActionExecuting.

            // No validation logic should be here for ActionExecutedContext.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var validationService = new ValidationAttributesService();

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var errors = validationService.Validate(argument);

                if (errors.Any())
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        Message = "Erro de validação",
                        Errors = errors
                    });
                    return;
                }
            }
        }
    }
}
