-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : ven. 28 nov. 2025 à 12:09
-- Version du serveur : 10.4.32-MariaDB
-- Version de PHP : 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `ecofood`
--

-- --------------------------------------------------------

--
-- Structure de la table `categorie`
--

CREATE TABLE `categorie` (
  `IdCategorie` int(11) NOT NULL,
  `NomCategorie` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `categorie`
--

INSERT INTO `categorie` (`IdCategorie`, `NomCategorie`) VALUES
(2, 'food');

-- --------------------------------------------------------

--
-- Structure de la table `message`
--

CREATE TABLE `message` (
  `IdMessage` int(11) NOT NULL,
  `Texte` text NOT NULL,
  `DateEnvoi` datetime DEFAULT current_timestamp(),
  `IdSender` int(11) NOT NULL,
  `IdReceiver` int(11) NOT NULL,
  `IdProduit` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `produit`
--

CREATE TABLE `produit` (
  `IdProduit` int(11) NOT NULL,
  `Titre` varchar(200) NOT NULL,
  `Description` text DEFAULT NULL,
  `Prix` decimal(10,2) NOT NULL,
  `DateExpiration` date DEFAULT NULL,
  `ImageUrl` varchar(255) DEFAULT NULL,
  `Statut` enum('Disponible','Reserve','Vendu') DEFAULT 'Disponible',
  `IdUser` int(11) NOT NULL,
  `IdCategorie` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `rating`
--

CREATE TABLE `rating` (
  `IdRating` int(11) NOT NULL,
  `Note` int(11) DEFAULT NULL CHECK (`Note` >= 1 and `Note` <= 5),
  `Commentaire` text DEFAULT NULL,
  `DateRating` datetime DEFAULT current_timestamp(),
  `IdFromUser` int(11) NOT NULL,
  `IdToUser` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `reservation`
--

CREATE TABLE `reservation` (
  `IdReservation` int(11) NOT NULL,
  `Statut` enum('EnAttente','Confirmee','Annulee') DEFAULT 'EnAttente',
  `DateReservation` datetime DEFAULT current_timestamp(),
  `IdProduit` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

CREATE TABLE `user` (
  `IdUser` int(11) NOT NULL,
  `Nom` varchar(100) NOT NULL,
  `Prenom` varchar(100) NOT NULL,
  `MotDePasse` varchar(255) NOT NULL,
  `Ville` varchar(100) DEFAULT NULL,
  `MoyenneRating` decimal(3,2) DEFAULT NULL,
  `TotalRatingsRecus` int(11) DEFAULT 0,
  `TotalRatingsDonnes` int(11) DEFAULT 0,
  `Role` enum('Admin','User') DEFAULT 'User',
  `Email` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`IdUser`, `Nom`, `Prenom`, `MotDePasse`, `Ville`, `MoyenneRating`, `TotalRatingsRecus`, `TotalRatingsDonnes`, `Role`, `Email`) VALUES
(1, 'dhahri', 'ichrak', '$2a$11$xauecD8v8jd0BY5mJfd3ROV96Fpgc0Emm0JpbS8ou5E8xwDA/VaOC', 'tunis', 0.00, 0, 0, 'User', 'ichrak1@example.com'),
(2, 'manel', 'mbarek', '$2a$11$oS12sUahb3yTaEXHF7tor.D3CDaI4ZycZ14//hooecMABQtiW/PYa', 'Tunis', 0.00, 0, 0, 'Admin', 'manel@example.com');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `categorie`
--
ALTER TABLE `categorie`
  ADD PRIMARY KEY (`IdCategorie`);

--
-- Index pour la table `message`
--
ALTER TABLE `message`
  ADD PRIMARY KEY (`IdMessage`),
  ADD KEY `FK_Message_Sender` (`IdSender`),
  ADD KEY `FK_Message_Receiver` (`IdReceiver`),
  ADD KEY `idx_message_chat` (`IdProduit`,`IdSender`,`IdReceiver`);

--
-- Index pour la table `produit`
--
ALTER TABLE `produit`
  ADD PRIMARY KEY (`IdProduit`),
  ADD KEY `FK_Produit_User` (`IdUser`),
  ADD KEY `FK_Produit_Categorie` (`IdCategorie`);

--
-- Index pour la table `rating`
--
ALTER TABLE `rating`
  ADD PRIMARY KEY (`IdRating`),
  ADD KEY `FK_Rating_FromUser` (`IdFromUser`),
  ADD KEY `FK_Rating_ToUser` (`IdToUser`);

--
-- Index pour la table `reservation`
--
ALTER TABLE `reservation`
  ADD PRIMARY KEY (`IdReservation`),
  ADD KEY `FK_Reservation_Produit` (`IdProduit`),
  ADD KEY `FK_Reservation_User` (`IdUser`);

--
-- Index pour la table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`IdUser`),
  ADD UNIQUE KEY `UQ_Users_Email` (`Email`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `categorie`
--
ALTER TABLE `categorie`
  MODIFY `IdCategorie` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT pour la table `message`
--
ALTER TABLE `message`
  MODIFY `IdMessage` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `produit`
--
ALTER TABLE `produit`
  MODIFY `IdProduit` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `rating`
--
ALTER TABLE `rating`
  MODIFY `IdRating` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `reservation`
--
ALTER TABLE `reservation`
  MODIFY `IdReservation` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `user`
--
ALTER TABLE `user`
  MODIFY `IdUser` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `message`
--
ALTER TABLE `message`
  ADD CONSTRAINT `FK_Message_Produit` FOREIGN KEY (`IdProduit`) REFERENCES `produit` (`IdProduit`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Message_Receiver` FOREIGN KEY (`IdReceiver`) REFERENCES `user` (`IdUser`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Message_Sender` FOREIGN KEY (`IdSender`) REFERENCES `user` (`IdUser`) ON DELETE CASCADE;

--
-- Contraintes pour la table `produit`
--
ALTER TABLE `produit`
  ADD CONSTRAINT `FK_Produit_Categorie` FOREIGN KEY (`IdCategorie`) REFERENCES `categorie` (`IdCategorie`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Produit_User` FOREIGN KEY (`IdUser`) REFERENCES `user` (`IdUser`) ON DELETE CASCADE;

--
-- Contraintes pour la table `rating`
--
ALTER TABLE `rating`
  ADD CONSTRAINT `FK_Rating_FromUser` FOREIGN KEY (`IdFromUser`) REFERENCES `user` (`IdUser`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Rating_ToUser` FOREIGN KEY (`IdToUser`) REFERENCES `user` (`IdUser`) ON DELETE CASCADE;

--
-- Contraintes pour la table `reservation`
--
ALTER TABLE `reservation`
  ADD CONSTRAINT `FK_Reservation_Produit` FOREIGN KEY (`IdProduit`) REFERENCES `produit` (`IdProduit`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Reservation_User` FOREIGN KEY (`IdUser`) REFERENCES `user` (`IdUser`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
