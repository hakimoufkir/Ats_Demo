import { Component, OnInit, Signal, signal, WritableSignal } from '@angular/core';
import { Table, TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ConfirmationService, MessageService } from 'primeng/api';
import { EmployeeService } from '../../services/employee.service';
import { EmployeeDetailsDto } from '../../interfaces/employee-details-dto';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, TableModule, ConfirmDialogModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent implements OnInit {
  //virtualEmployees: EmployeeDetailsDto[] = [];
  //totalRecords: number = 0;
  // loading: boolean = false;
  virtualEmployees: WritableSignal<EmployeeDetailsDto[]> = signal<EmployeeDetailsDto[]>([]);
  totalRecords: WritableSignal<number> = signal<number>(0);
  loading: WritableSignal<boolean> = signal<boolean>(false);

  cols: any = [
    { field: 'name', header: 'Name' },
    { field: 'position', header: 'Position' },
    { field: 'office', header: 'Office' },
    { field: 'age', header: 'Age' },
    { field: 'salary', header: 'Salary' }
  ];

  constructor(
    private employeeService: EmployeeService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // this.virtualEmployees = Array.from({ length: 10000 });
    this.virtualEmployees.set(Array.from({ length: 10000 }) as EmployeeDetailsDto[]);
  }

  /**
   * Lazy loading function to fetch employees when scrolling
   */
  loadEmployeesLazy(event: TableLazyLoadEvent): void {
    const start = event.first || 0;
    const limit = event.rows ?? 10;

    this.loading.set(true);

    this.employeeService.getEmployeesPaged(start, limit).subscribe(
      response => {
        const filteredData = response.filter(emp => emp?.name);

        // Preserve existing items, only replace specific indices
        const currentData = this.virtualEmployees();
        currentData.splice(start, filteredData.length, ...filteredData);
        this.virtualEmployees.set([...currentData]); // Update reference

        this.totalRecords.set(response.length); // Ensure total records come from API
        this.loading.set(false);
      },
      error => {
        this.loading.set(false);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load employees.', life: 3000 });
      }
    );
  }



  /**
   * Navigate to edit employee page
   */
  editEmployee(employee: EmployeeDetailsDto): void {
    this.router.navigate(['/employees', employee.id]);
  }

  /**
   * Confirm before deleting an employee
   */
  confirmDelete(employee: EmployeeDetailsDto): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete <strong>${employee.name}</strong>?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Yes',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button p-confirm-dialog-accept',
      rejectButtonStyleClass: 'p-button p-confirm-dialog-reject',
      accept: () => this.deleteEmployee(employee.id!)
    });
  }

  /**
   * Delete an employee and update the UI
   */
  deleteEmployee(idRemove: string): void {
    console.log(idRemove);
    this.employeeService.deleteEmployee(idRemove).subscribe(
      () => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Employee deleted successfully.', life: 3000 });

        // Ensure virtualEmployees does not contain undefined values before filtering
        this.virtualEmployees.set(this.virtualEmployees().filter(emp => emp));
      },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to delete employee.', life: 3000 });
      }
    );
  }
}