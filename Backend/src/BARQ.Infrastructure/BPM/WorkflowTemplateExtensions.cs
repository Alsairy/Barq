using System;
using System.Threading.Tasks;
using BARQ.Core.Entities;
using BARQ.Core.Enums;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public static class WorkflowTemplateExtensions
    {
        /// <summary>
        /// </summary>
        public static string GetBpmnProcessId(this WorkflowTemplate template)
        {
            return $"process_{template.WorkflowType}_{template.Id}".Replace("-", "_").ToLower();
        }

        /// <summary>
        /// </summary>
        public static string GetBpmnProcessName(this WorkflowTemplate template)
        {
            return $"{template.Name} v{template.Version}";
        }

        /// <summary>
        /// </summary>
        public static bool IsBpmnFormat(this WorkflowTemplate template)
        {
            if (string.IsNullOrWhiteSpace(template.WorkflowDefinition))
                return false;

            return template.WorkflowDefinition.TrimStart().StartsWith("<?xml") &&
                   template.WorkflowDefinition.Contains("http://www.omg.org/spec/BPMN/20100524/MODEL");
        }

        /// <summary>
        /// </summary>
        public static bool IsJsonFormat(this WorkflowTemplate template)
        {
            if (string.IsNullOrWhiteSpace(template.WorkflowDefinition))
                return false;

            var trimmed = template.WorkflowDefinition.TrimStart();
            return trimmed.StartsWith("{") || trimmed.StartsWith("[");
        }

        /// <summary>
        /// </summary>
        public static async Task<bool> MigrateToBpmnAsync(this WorkflowTemplate template, IBpmnMigrationService migrationService)
        {
            try
            {
                if (template.IsBpmnFormat())
                {
                    return true;
                }

                var processId = template.GetBpmnProcessId();
                var processName = template.GetBpmnProcessName();

                var bpmnDefinition = await migrationService.MigrateJsonToBpmnAsync(
                    template.WorkflowDefinition, 
                    processId, 
                    processName);

                var validationResult = await migrationService.ValidateBpmnAsync(bpmnDefinition);
                if (!validationResult.IsValid)
                {
                    return false;
                }

                template.WorkflowDefinition = bpmnDefinition;
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public static string GetSampleBpmnDefinition(WorkflowType workflowType)
        {
            return workflowType switch
            {
                WorkflowType.AITaskApproval => GetAIRequestBpmnSample(),
                WorkflowType.BRDApproval => GetApprovalBpmnSample(),
                WorkflowType.ProjectApproval => GetProjectManagementBpmnSample(),
                _ => GetGenericBpmnSample()
            };
        }

        private static string GetAIRequestBpmnSample()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?>
<definitions xmlns=""http://www.omg.org/spec/BPMN/20100524/MODEL"" 
             xmlns:flowable=""http://flowable.org/bpmn""
             targetNamespace=""http://barq.ai/workflows""
             id=""ai_request_process_definitions"">
  
  <process id=""ai_request_standard_process"" name=""AI Request Standard Processing"" isExecutable=""true"">
    
    <!-- Start Event -->
    <startEvent id=""startEvent"" name=""AI Request Submitted"">
      <extensionElements>
        <flowable:executionListener event=""start"" class=""BARQ.Workflows.Listeners.WorkflowStartListener""/>
      </extensionElements>
    </startEvent>
    
    <!-- Request Validation Task -->
    <serviceTask id=""requestValidation"" name=""Request Validation"" 
                 flowable:class=""BARQ.Workflows.Tasks.RequestValidationTask"">
      <extensionElements>
        <flowable:field name=""validationRules"">
          <flowable:expression>${validationRules}</flowable:expression>
        </flowable:field>
      </extensionElements>
    </serviceTask>
    
    <!-- Manager Approval Task -->
    <userTask id=""managerApproval"" name=""Manager Approval"" 
              flowable:candidateGroups=""managers"">
      <extensionElements>
        <flowable:taskListener event=""create"" class=""BARQ.Workflows.Listeners.TaskAssignmentListener""/>
      </extensionElements>
    </userTask>
    
    <!-- AI Processing Task -->
    <serviceTask id=""aiProcessing"" name=""AI Processing"" 
                 flowable:class=""BARQ.Workflows.Tasks.AIProcessingTask"">
      <extensionElements>
        <flowable:field name=""aiProvider"">
          <flowable:expression>${aiProvider}</flowable:expression>
        </flowable:field>
      </extensionElements>
    </serviceTask>
    
    <!-- End Event -->
    <endEvent id=""endEvent"" name=""Request Completed""/>
    
    <!-- Sequence Flows -->
    <sequenceFlow id=""flow1"" sourceRef=""startEvent"" targetRef=""requestValidation""/>
    <sequenceFlow id=""flow2"" sourceRef=""requestValidation"" targetRef=""managerApproval""/>
    <sequenceFlow id=""flow3"" sourceRef=""managerApproval"" targetRef=""aiProcessing""/>
    <sequenceFlow id=""flow4"" sourceRef=""aiProcessing"" targetRef=""endEvent""/>
    
  </process>
</definitions>";
        }

        private static string GetApprovalBpmnSample()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?>
<definitions xmlns=""http://www.omg.org/spec/BPMN/20100524/MODEL"" 
             xmlns:flowable=""http://flowable.org/bpmn""
             targetNamespace=""http://barq.ai/workflows""
             id=""approval_process_definitions"">
  
  <process id=""standard_approval_process"" name=""Standard Approval Process"" isExecutable=""true"">
    
    <startEvent id=""startEvent"" name=""Request Submitted""/>
    
    <userTask id=""initialReview"" name=""Initial Review"" 
              flowable:candidateGroups=""reviewers""/>
    
    <exclusiveGateway id=""approvalDecision"" name=""Approval Decision""/>
    
    <userTask id=""managerApproval"" name=""Manager Approval"" 
              flowable:candidateGroups=""managers""/>
    
    <endEvent id=""approvedEnd"" name=""Request Approved""/>
    <endEvent id=""rejectedEnd"" name=""Request Rejected""/>
    
    <sequenceFlow id=""flow1"" sourceRef=""startEvent"" targetRef=""initialReview""/>
    <sequenceFlow id=""flow2"" sourceRef=""initialReview"" targetRef=""approvalDecision""/>
    <sequenceFlow id=""flow3"" sourceRef=""approvalDecision"" targetRef=""managerApproval"">
      <conditionExpression>${approved == true}</conditionExpression>
    </sequenceFlow>
    <sequenceFlow id=""flow4"" sourceRef=""approvalDecision"" targetRef=""rejectedEnd"">
      <conditionExpression>${approved == false}</conditionExpression>
    </sequenceFlow>
    <sequenceFlow id=""flow5"" sourceRef=""managerApproval"" targetRef=""approvedEnd""/>
    
  </process>
</definitions>";
        }

        private static string GetProjectManagementBpmnSample()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?>
<definitions xmlns=""http://www.omg.org/spec/BPMN/20100524/MODEL"" 
             xmlns:flowable=""http://flowable.org/bpmn""
             targetNamespace=""http://barq.ai/workflows""
             id=""project_management_definitions"">
  
  <process id=""project_lifecycle_process"" name=""Project Lifecycle Management"" isExecutable=""true"">
    
    <startEvent id=""startEvent"" name=""Project Initiated""/>
    
    <userTask id=""projectPlanning"" name=""Project Planning"" 
              flowable:candidateGroups=""project_managers""/>
    
    <userTask id=""resourceAllocation"" name=""Resource Allocation"" 
              flowable:candidateGroups=""resource_managers""/>
    
    <userTask id=""projectExecution"" name=""Project Execution"" 
              flowable:candidateGroups=""project_team""/>
    
    <userTask id=""projectReview"" name=""Project Review"" 
              flowable:candidateGroups=""stakeholders""/>
    
    <endEvent id=""endEvent"" name=""Project Completed""/>
    
    <sequenceFlow id=""flow1"" sourceRef=""startEvent"" targetRef=""projectPlanning""/>
    <sequenceFlow id=""flow2"" sourceRef=""projectPlanning"" targetRef=""resourceAllocation""/>
    <sequenceFlow id=""flow3"" sourceRef=""resourceAllocation"" targetRef=""projectExecution""/>
    <sequenceFlow id=""flow4"" sourceRef=""projectExecution"" targetRef=""projectReview""/>
    <sequenceFlow id=""flow5"" sourceRef=""projectReview"" targetRef=""endEvent""/>
    
  </process>
</definitions>";
        }

        private static string GetGenericBpmnSample()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?>
<definitions xmlns=""http://www.omg.org/spec/BPMN/20100524/MODEL"" 
             xmlns:flowable=""http://flowable.org/bpmn""
             targetNamespace=""http://barq.ai/workflows""
             id=""generic_process_definitions"">
  
  <process id=""generic_workflow_process"" name=""Generic Workflow Process"" isExecutable=""true"">
    
    <startEvent id=""startEvent"" name=""Process Started""/>
    
    <userTask id=""reviewTask"" name=""Review Task"" 
              flowable:candidateGroups=""users""/>
    
    <endEvent id=""endEvent"" name=""Process Completed""/>
    
    <sequenceFlow id=""flow1"" sourceRef=""startEvent"" targetRef=""reviewTask""/>
    <sequenceFlow id=""flow2"" sourceRef=""reviewTask"" targetRef=""endEvent""/>
    
  </process>
</definitions>";
        }
    }
}
