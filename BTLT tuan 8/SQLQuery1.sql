CREATE TABLE SinhVien (
    MaSV NVARCHAR(10) PRIMARY KEY,
    TenSV NVARCHAR(50),
    GioiTinh NVARCHAR(5),
    NgaySinh DATETIME,
    QueQuan NVARCHAR(100),
    MaLop NVARCHAR(10)
);

INSERT INTO SinhVien VALUES 
('SV01', N'Nguyễn Văn An', N'Nam', '2003-05-15', N'Hà Nội', 'CNTT1'),
('SV02', N'Trần Thị Bình', N'Nữ', '2004-03-11', N'Nam Định', 'CNTT2');
