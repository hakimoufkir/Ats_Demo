import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployeeService } from '../../services/employee.service';
import { ActivatedRoute, Router } from '@angular/router';
import {UpdateEmployeeDto} from '../../interfaces/update-employee-dto';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { InputNumber } from 'primeng/inputnumber';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  templateUrl: './employee-form.component.html',
  styleUrl: './employee-form.component.css',
  imports: [ReactiveFormsModule,ToastModule, InputNumber,CommonModule]
})
export class EmployeeFormComponent implements OnInit {
  employeeForm!: FormGroup;
  isEditMode: boolean = false;
  employeeId?: string;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private confirmationService :ConfirmationService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    console.log("Sefyan mara min houna")
    this.route.paramMap.subscribe(params => {
      this.employeeId = params.get('id')!;
      if (this.employeeId) {
        this.isEditMode = true;
        this.loadEmployee(this.employeeId);
      }
    });
  }

  initializeForm(): void {
    this.employeeForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      position: ['', Validators.required],
      office: ['', Validators.required],
      age: [null, [Validators.required, Validators.min(18), Validators.max(65)]],
      salary: [null, [Validators.required, Validators.min(1000)]]
    });
  }

  loadEmployee(id: string): void {
    this.employeeService.getEmployeeById(id).subscribe(employee => {
      this.employeeForm.patchValue(employee);
    });
  }

  submitForm(): void {
    if (this.employeeForm.invalid) return;

    const employeeDto:UpdateEmployeeDto = this.employeeForm.value;

    if (this.isEditMode && this.employeeId) {
      const updateEmployeeDto : UpdateEmployeeDto = this.employeeForm.value;
      this.employeeService.updateEmployee(this.employeeId,updateEmployeeDto).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Employee Updated successfully.', life: 3000 });
        this.router.navigate(['/employees']);
      });
    } else {
      this.employeeService.createEmployee(employeeDto).subscribe(() => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: `Failed to update employee with id ${this.employeeId}.` , life: 3000 });
        this.router.navigate(['/employees']);
      });
    }
  }
}
