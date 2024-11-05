import { FileDto } from "./file.model";

export interface Task {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: Date;
  attachmentKey?: string; 
}

export interface TaskDto {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  attachmentDto?: FileDto;
}