import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeDetailsDto } from '../interfaces/employee-details-dto';
import {CreateEmployeeDto} from '../interfaces/create-employee-dto';
import {UpdateEmployeeDto} from '../interfaces/update-employee-dto';
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private baseUrl = 'https://localhost:7214/api/employees';

  constructor(private http: HttpClient) { }

  getEmployeesPaged(startIndex: number, pageSize: number): Observable<EmployeeDetailsDto[]> {
    return this.http.get<EmployeeDetailsDto[]>(`${this.baseUrl}?start=${startIndex}&limit=${pageSize}`);
  }

  getAllEmployees(): Observable<EmployeeDetailsDto[]> {
    return this.http.get<EmployeeDetailsDto[]>(this.baseUrl);
  }

  getEmployeeById(id: string): Observable<EmployeeDetailsDto> {
    return this.http.get<EmployeeDetailsDto>(`${this.baseUrl}/${id}`);
  }

  createEmployee(employeeDto: CreateEmployeeDto): Observable<EmployeeDetailsDto> {
    return this.http.post<EmployeeDetailsDto>(this.baseUrl, employeeDto);
  }

  updateEmployee(id: string, updateEmployeeDto: UpdateEmployeeDto): Observable<EmployeeDetailsDto> {
    return this.http.put<EmployeeDetailsDto>(`${this.baseUrl}/${id}`, updateEmployeeDto);
  }

  deleteEmployee(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

