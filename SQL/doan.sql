-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th12 06, 2021 lúc 12:35 PM
-- Phiên bản máy phục vụ: 10.4.21-MariaDB
-- Phiên bản PHP: 8.0.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `doan`
--

DELIMITER $$
--
-- Các hàm
--
CREATE DEFINER=`root`@`localhost` FUNCTION `CK_FOLLOW` (`A` INT, `B` INT) RETURNS TINYINT(1) BEGIN
	DECLARE CNT INT;
    SET CNT = 0;
    
    SELECT COUNT(*) 
    INTO CNT 
    FROM follower
    where uida = A AND uidb = B;
    
    IF (CNT != 0) THEN
		RETURN TRUE;
	END IF;
	RETURN FALSE;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `belong`
--

CREATE TABLE `belong` (
  `qid` int(11) NOT NULL,
  `mcid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `comment`
--

CREATE TABLE `comment` (
  `pid` int(11) DEFAULT NULL,
  `id` int(11) NOT NULL,
  `description` text COLLATE utf8_unicode_ci NOT NULL,
  `mcid` int(11) NOT NULL,
  `owner` int(11) NOT NULL,
  `greentick` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `follower`
--

CREATE TABLE `follower` (
  `uida` int(11) NOT NULL,
  `uidb` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `interaction`
--

CREATE TABLE `interaction` (
  `uid` int(11) NOT NULL,
  `mcid` int(11) NOT NULL,
  `favorite` tinyint(1) NOT NULL DEFAULT 0,
  `score` tinyint(3) UNSIGNED NOT NULL,
  `done` smallint(5) UNSIGNED NOT NULL DEFAULT 0
) ;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `multiplechoice`
--

CREATE TABLE `multiplechoice` (
  `id` int(11) NOT NULL,
  `title` varchar(256) COLLATE utf8_unicode_ci NOT NULL,
  `description` text COLLATE utf8_unicode_ci DEFAULT NULL,
  `time` tinyint(3) UNSIGNED NOT NULL,
  `total` tinyint(3) UNSIGNED NOT NULL,
  `participant` int(10) UNSIGNED NOT NULL DEFAULT 0
) ;

--
-- Đang đổ dữ liệu cho bảng `multiplechoice`
--

INSERT INTO `multiplechoice` (`id`, `title`, `description`, `rating`, `time`, `total`, `status`, `upvote`, `participant`, `createDate`) VALUES
(1, 'Tiếng anh mỗi ngày', 'Làm trắc nghiệm tiếng anh mỗi ngày', '2.5', 30, 10, 0, 0, 0, '2021-11-23');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `question`
--

CREATE TABLE `question` (
  `id` int(11) NOT NULL,
  `question` text COLLATE utf8_unicode_ci NOT NULL,
  `filter` text COLLATE utf8_unicode_ci NOT NULL,
  `A` text COLLATE utf8_unicode_ci NOT NULL,
  `B` text COLLATE utf8_unicode_ci NOT NULL,
  `C` text COLLATE utf8_unicode_ci NOT NULL,
  `D` text COLLATE utf8_unicode_ci NOT NULL,
  `answer` varchar(1) COLLATE utf8_unicode_ci NOT NULL,
  `uid` int(11) NOT NULL
) ;

--
-- Đang đổ dữ liệu cho bảng `question`
--

INSERT INTO `question` (`id`, `question`, `filter`, `A`, `B`, `C`, `D`, `answer`, `uid`) VALUES
(1, 'Delectable', '', 'ngon', 'dỡ', 'bảng', 'vui', 'A', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tag`
--

CREATE TABLE `tag` (
  `tagname` varchar(30) COLLATE utf8_unicode_ci NOT NULL,
  `qid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `name` varchar(256) COLLATE utf8_unicode_ci NOT NULL,
  `upvote` smallint(11) DEFAULT 0,
  `greentick` smallint(11) UNSIGNED DEFAULT 0,
  `account` varchar(20) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(20) COLLATE utf8_unicode_ci NOT NULL
) ;

--
-- Đang đổ dữ liệu cho bảng `user`
--

INSERT INTO `user` (`id`, `name`, `upvote`, `greentick`, `account`, `password`) VALUES
(1, 'Trịnh Huỳnh Đăng', 0, 0, 'dangth112001', '1234567890');
INSERT INTO `user` (`id`, `name`, `upvote`, `greentick`, `account`, `password`) VALUES
(2, 'Nguyễn Tấn Ngà', 0, 0, 'ngant162001', '1234567890');
INSERT INTO `user` (`id`, `name`, `upvote`, `greentick`, `account`, `password`) VALUES
(3, 'Trịnh Huỳnh Đăng', 0, 0, 'nhanhn2001', '1234567890');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `belong`
--
ALTER TABLE `belong`
  ADD PRIMARY KEY (`qid`,`mcid`),
  ADD KEY `FK_belong_mcid` (`mcid`);

--
-- Chỉ mục cho bảng `comment`
--
ALTER TABLE `comment`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_comment_owner` (`owner`),
  ADD KEY `FK_comment_pid` (`pid`),
  ADD KEY `FK_comment_mcid` (`mcid`);

--
-- Chỉ mục cho bảng `follower`
--
ALTER TABLE `follower`
  ADD KEY `FK_follower_uida` (`uida`),
  ADD KEY `FK_follower_uidb` (`uidb`);

--
-- Chỉ mục cho bảng `interaction`
--
ALTER TABLE `interaction`
  ADD PRIMARY KEY (`mcid`,`uid`),
  ADD KEY `FK_interaction_uid` (`uid`);

--
-- Chỉ mục cho bảng `multiplechoice`
--
ALTER TABLE `multiplechoice`
  ADD PRIMARY KEY (`id`);

--
-- Chỉ mục cho bảng `question`
--
ALTER TABLE `question`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_question_uid` (`uid`);

--
-- Chỉ mục cho bảng `tag`
--
ALTER TABLE `tag`
  ADD PRIMARY KEY (`tagname`,`qid`),
  ADD KEY `FK_tag_qid` (`qid`);

--
-- Chỉ mục cho bảng `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `UQ_Account` (`account`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `comment`
--
ALTER TABLE `comment`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `multiplechoice`
--
ALTER TABLE `multiplechoice`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `question`
--
ALTER TABLE `question`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `belong`
--
ALTER TABLE `belong`
  ADD CONSTRAINT `FK_belong_mcid` FOREIGN KEY (`mcid`) REFERENCES `multiplechoice` (`id`),
  ADD CONSTRAINT `FK_belong_qid` FOREIGN KEY (`qid`) REFERENCES `question` (`id`);

--
-- Các ràng buộc cho bảng `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `FK_comment_mcid` FOREIGN KEY (`mcid`) REFERENCES `multiplechoice` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_comment_owner` FOREIGN KEY (`owner`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_comment_pid` FOREIGN KEY (`pid`) REFERENCES `comment` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `follower`
--
ALTER TABLE `follower`
  ADD CONSTRAINT `FK_follower_uida` FOREIGN KEY (`uida`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_follower_uidb` FOREIGN KEY (`uidb`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `interaction`
--
ALTER TABLE `interaction`
  ADD CONSTRAINT `FK_interaction_mcid` FOREIGN KEY (`mcid`) REFERENCES `multiplechoice` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_interaction_uid` FOREIGN KEY (`uid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `question`
--
ALTER TABLE `question`
  ADD CONSTRAINT `FK_question_uid` FOREIGN KEY (`uid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `tag`
--
ALTER TABLE `tag`
  ADD CONSTRAINT `FK_tag_qid` FOREIGN KEY (`qid`) REFERENCES `question` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
